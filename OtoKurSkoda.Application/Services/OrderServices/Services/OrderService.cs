using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.OrderServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;

namespace OtoKurSkoda.Application.Services.OrderServices.Services
{
    public class OrderService : BaseApplicationService, IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> CreateOrderFromCartAsync(Guid userId, CreateOrderRequest request)
        {
            var cartRepo = _unitOfWork.GetRepository<Cart>();
            var cart = await cartRepo.GetWhere(x => x.UserId == userId && x.Status)
                .Include(x => x.Items.Where(i => i.Status))
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images.Where(img => img.IsMain && img.Status))
                .FirstOrDefaultAsync();

            if (cart == null || !cart.Items.Any())
                return ErrorResult("CART_EMPTY", "Sepetiniz boş.");

            // Stok kontrolü
            foreach (var item in cart.Items)
            {
                if (item.Product.StockQuantity < item.Quantity)
                    return ErrorResult("INSUFFICIENT_STOCK", $"'{item.Product.Name}' için yetersiz stok. Mevcut: {item.Product.StockQuantity}");
            }

            // Sipariş numarası oluştur
            var orderNumber = GenerateOrderNumber();

            // Sipariş oluştur
            var order = new Order
            {
                OrderNumber = orderNumber,
                UserId = userId,
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,

                // Teslimat Bilgileri
                ShippingFirstName = request.ShippingFirstName,
                ShippingLastName = request.ShippingLastName,
                ShippingPhone = request.ShippingPhone,
                ShippingEmail = request.ShippingEmail,
                ShippingAddress = request.ShippingAddress,
                ShippingCity = request.ShippingCity,
                ShippingDistrict = request.ShippingDistrict,
                ShippingPostalCode = request.ShippingPostalCode,

                // Fatura Bilgileri
                BillingFirstName = request.UseSameAddressForBilling ? request.ShippingFirstName : request.BillingFirstName,
                BillingLastName = request.UseSameAddressForBilling ? request.ShippingLastName : request.BillingLastName,
                BillingPhone = request.UseSameAddressForBilling ? request.ShippingPhone : request.BillingPhone,
                BillingAddress = request.UseSameAddressForBilling ? request.ShippingAddress : request.BillingAddress,
                BillingCity = request.UseSameAddressForBilling ? request.ShippingCity : request.BillingCity,
                BillingDistrict = request.UseSameAddressForBilling ? request.ShippingDistrict : request.BillingDistrict,
                BillingPostalCode = request.UseSameAddressForBilling ? request.ShippingPostalCode : request.BillingPostalCode,
                TaxNumber = request.TaxNumber,
                CompanyName = request.CompanyName,

                CustomerNote = request.CustomerNote,

                Items = new List<OrderItem>()
            };

            decimal subTotal = 0;
            decimal taxAmount = 0;

            foreach (var cartItem in cart.Items)
            {
                var product = cartItem.Product;
                var unitPrice = product.DiscountedPrice ?? product.Price;
                var itemTaxAmount = unitPrice * (product.TaxRate / 100) * cartItem.Quantity;
                var itemTotal = unitPrice * cartItem.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductSku = product.Sku,
                    ProductOemNumber = product.OemNumber,
                    ProductImageUrl = product.Images.FirstOrDefault()?.Url,
                    Quantity = cartItem.Quantity,
                    UnitPrice = unitPrice,
                    DiscountAmount = 0,
                    TaxRate = product.TaxRate.Value,
                    TaxAmount = itemTaxAmount.Value,
                    TotalPrice = itemTotal
                };

                order.Items.Add(orderItem);
                subTotal += itemTotal;
                taxAmount += itemTaxAmount.Value;

                // Stok düş
                product.StockQuantity -= cartItem.Quantity;
                product.SoldCount += cartItem.Quantity;
                _unitOfWork.GetRepository<Product>().Update(product);
            }

            order.SubTotal = subTotal;
            order.TaxAmount = taxAmount;
            order.ShippingCost = 0; // Kargo ücreti sonra hesaplanabilir
            order.DiscountAmount = 0;
            order.TotalAmount = subTotal + taxAmount + order.ShippingCost - order.DiscountAmount;

            var orderRepo = _unitOfWork.GetRepository<Order>();
            await orderRepo.AddAsync(order);

            // Sepeti temizle
            var cartItemRepo = _unitOfWork.GetRepository<CartItem>();
            foreach (var item in cart.Items)
            {
                item.Status = false;
                cartItemRepo.Update(item);
            }

            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new { OrderId = order.Id, OrderNumber = order.OrderNumber }, "ORDER_CREATED", "Siparişiniz oluşturuldu.");
        }

        public async Task<ServiceResult> GetMyOrdersAsync(Guid userId, int pageNumber = 1, int pageSize = 20)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();

            var query = orderRepo.GetWhere(x => x.UserId == userId && x.Status)
                .Include(x => x.Items)
                .OrderByDescending(x => x.CreateTime);

            var totalCount = await query.CountAsync();

            var orders = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = orders.Select(MapToListDto).ToList();

            return SuccessPagedDataResult(result, totalCount, pageNumber, pageSize, "ORDERS_LISTED", "Siparişler listelendi.");
        }

        public async Task<ServiceResult> GetMyOrderDetailAsync(Guid userId, Guid orderId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();

            var order = await orderRepo.GetWhere(x => x.Id == orderId && x.UserId == userId && x.Status)
                .Include(x => x.User)
                .Include(x => x.Items)
                .FirstOrDefaultAsync();

            if (order == null)
                return ErrorResult("ORDER_NOT_FOUND", "Sipariş bulunamadı.");

            return SuccessDataResult(MapToDto(order), "ORDER_FOUND", "Sipariş bulundu.");
        }

        public async Task<ServiceResult> CancelOrderAsync(Guid userId, Guid orderId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();

            var order = await orderRepo.GetWhere(x => x.Id == orderId && x.UserId == userId && x.Status)
                .Include(x => x.Items)
                .FirstOrDefaultAsync();

            if (order == null)
                return ErrorResult("ORDER_NOT_FOUND", "Sipariş bulunamadı.");

            if (order.OrderStatus != OrderStatus.Pending && order.OrderStatus != OrderStatus.Confirmed)
                return ErrorResult("CANNOT_CANCEL", "Bu sipariş iptal edilemez. Sipariş durumu: " + GetStatusText(order.OrderStatus));

            // Siparişi iptal et
            order.OrderStatus = OrderStatus.Cancelled;
            order.CancelledAt = DateTime.UtcNow;

            // Stokları geri ekle
            var productRepo = _unitOfWork.GetRepository<Product>();
            foreach (var item in order.Items)
            {
                var product = await productRepo.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity += item.Quantity;
                    product.SoldCount -= item.Quantity;
                    productRepo.Update(product);
                }
            }

            orderRepo.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ORDER_CANCELLED", "Sipariş iptal edildi.");
        }

        public async Task<ServiceResult> GetAllOrdersAsync(OrderFilterRequest filter)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();

            var query = orderRepo.GetWhere(x => x.Status)
                .Include(x => x.User)
                .Include(x => x.Items)
                .AsQueryable();

            if (filter.UserId.HasValue)
                query = query.Where(x => x.UserId == filter.UserId);

            if (filter.Status.HasValue)
                query = query.Where(x => x.OrderStatus == filter.Status);

            if (filter.PaymentStatus.HasValue)
                query = query.Where(x => x.PaymentStatus == filter.PaymentStatus);

            if (filter.StartDate.HasValue)
                query = query.Where(x => x.CreateTime >= filter.StartDate);

            if (filter.EndDate.HasValue)
                query = query.Where(x => x.CreateTime <= filter.EndDate);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var search = filter.SearchTerm.ToLower();
                query = query.Where(x => x.OrderNumber.ToLower().Contains(search) ||
                                         x.ShippingFirstName.ToLower().Contains(search) ||
                                         x.ShippingLastName.ToLower().Contains(search) ||
                                         x.ShippingPhone.Contains(search));
            }

            query = query.OrderByDescending(x => x.CreateTime);

            var totalCount = await query.CountAsync();

            var orders = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = orders.Select(MapToListDto).ToList();

            return SuccessPagedDataResult(result, totalCount, filter.PageNumber, filter.PageSize, "ORDERS_LISTED", "Siparişler listelendi.");
        }

        public async Task<ServiceResult> GetOrderByIdAsync(Guid orderId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();

            var order = await orderRepo.GetWhere(x => x.Id == orderId && x.Status)
                .Include(x => x.User)
                .Include(x => x.Items)
                .FirstOrDefaultAsync();

            if (order == null)
                return ErrorResult("ORDER_NOT_FOUND", "Sipariş bulunamadı.");

            return SuccessDataResult(MapToDto(order), "ORDER_FOUND", "Sipariş bulundu.");
        }

        public async Task<ServiceResult> GetOrderByNumberAsync(string orderNumber)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();

            var order = await orderRepo.GetWhere(x => x.OrderNumber == orderNumber && x.Status)
                .Include(x => x.User)
                .Include(x => x.Items)
                .FirstOrDefaultAsync();

            if (order == null)
                return ErrorResult("ORDER_NOT_FOUND", "Sipariş bulunamadı.");

            return SuccessDataResult(MapToDto(order), "ORDER_FOUND", "Sipariş bulundu.");
        }

        public async Task<ServiceResult> UpdateOrderStatusAsync(UpdateOrderStatusRequest request)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();

            var order = await orderRepo.GetByIdAsync(request.OrderId);

            if (order == null || !order.Status)
                return ErrorResult("ORDER_NOT_FOUND", "Sipariş bulunamadı.");

            order.OrderStatus = request.NewStatus;

            if (!string.IsNullOrEmpty(request.AdminNote))
                order.AdminNote = request.AdminNote;

            switch (request.NewStatus)
            {
                case OrderStatus.Shipped:
                    order.ShippedAt = DateTime.UtcNow;
                    break;
                case OrderStatus.Delivered:
                    order.DeliveredAt = DateTime.UtcNow;
                    break;
                case OrderStatus.Cancelled:
                    order.CancelledAt = DateTime.UtcNow;
                    break;
            }

            orderRepo.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("STATUS_UPDATED", $"Sipariş durumu '{GetStatusText(request.NewStatus)}' olarak güncellendi.");
        }

        public async Task<ServiceResult> UpdateShippingInfoAsync(UpdateShippingInfoRequest request)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();

            var order = await orderRepo.GetByIdAsync(request.OrderId);

            if (order == null || !order.Status)
                return ErrorResult("ORDER_NOT_FOUND", "Sipariş bulunamadı.");

            order.CargoCompany = request.CargoCompany;
            order.TrackingNumber = request.TrackingNumber;

            if (order.OrderStatus == OrderStatus.Processing || order.OrderStatus == OrderStatus.Confirmed)
            {
                order.OrderStatus = OrderStatus.Shipped;
                order.ShippedAt = DateTime.UtcNow;
            }

            orderRepo.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("SHIPPING_UPDATED", "Kargo bilgileri güncellendi.");
        }

        public async Task<ServiceResult> GetOrderStatisticsAsync()
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();

            var today = DateTime.UtcNow.Date;
            var thisMonth = new DateTime(today.Year, today.Month, 1);

            var allOrders = await orderRepo.GetWhere(x => x.Status).ToListAsync();

            var stats = new
            {
                TotalOrders = allOrders.Count,
                TodayOrders = allOrders.Count(x => x.CreateTime.Date == today),
                ThisMonthOrders = allOrders.Count(x => x.CreateTime >= thisMonth),
                PendingOrders = allOrders.Count(x => x.OrderStatus == OrderStatus.Pending),
                ProcessingOrders = allOrders.Count(x => x.OrderStatus == OrderStatus.Processing || x.OrderStatus == OrderStatus.Confirmed),
                ShippedOrders = allOrders.Count(x => x.OrderStatus == OrderStatus.Shipped),
                DeliveredOrders = allOrders.Count(x => x.OrderStatus == OrderStatus.Delivered),
                CancelledOrders = allOrders.Count(x => x.OrderStatus == OrderStatus.Cancelled),
                TotalRevenue = allOrders.Where(x => x.PaymentStatus == PaymentStatus.Paid).Sum(x => x.TotalAmount),
                ThisMonthRevenue = allOrders.Where(x => x.PaymentStatus == PaymentStatus.Paid && x.CreateTime >= thisMonth).Sum(x => x.TotalAmount)
            };

            return SuccessDataResult(stats, "STATS_RETRIEVED", "İstatistikler alındı.");
        }

        private static string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }

        private static string GetStatusText(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "Beklemede",
                OrderStatus.Confirmed => "Onaylandı",
                OrderStatus.Processing => "Hazırlanıyor",
                OrderStatus.Shipped => "Kargoya Verildi",
                OrderStatus.Delivered => "Teslim Edildi",
                OrderStatus.Cancelled => "İptal Edildi",
                OrderStatus.Returned => "İade Edildi",
                _ => "Bilinmiyor"
            };
        }

        private static string GetPaymentStatusText(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "Ödeme Bekleniyor",
                PaymentStatus.Paid => "Ödendi",
                PaymentStatus.Failed => "Ödeme Başarısız",
                PaymentStatus.Refunded => "İade Edildi",
                PaymentStatus.PartialRefund => "Kısmi İade",
                _ => "Bilinmiyor"
            };
        }

        private OrderListDto MapToListDto(Order o) => new()
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            OrderStatus = o.OrderStatus,
            OrderStatusText = GetStatusText(o.OrderStatus),
            PaymentStatus = o.PaymentStatus,
            PaymentStatusText = GetPaymentStatusText(o.PaymentStatus),
            TotalAmount = o.TotalAmount,
            ItemCount = o.Items?.Count ?? 0,
            CreateTime = o.CreateTime
        };

        private OrderDto MapToDto(Order o) => new()
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            UserId = o.UserId,
            UserName = o.User != null ? $"{o.User.FirstName} {o.User.LastName}" : null,
            UserEmail = o.User?.Email,
            OrderStatus = o.OrderStatus,
            OrderStatusText = GetStatusText(o.OrderStatus),
            PaymentStatus = o.PaymentStatus,
            PaymentStatusText = GetPaymentStatusText(o.PaymentStatus),
            SubTotal = o.SubTotal,
            ShippingCost = o.ShippingCost,
            TaxAmount = o.TaxAmount,
            DiscountAmount = o.DiscountAmount,
            TotalAmount = o.TotalAmount,
            ShippingFirstName = o.ShippingFirstName,
            ShippingLastName = o.ShippingLastName,
            ShippingPhone = o.ShippingPhone,
            ShippingEmail = o.ShippingEmail,
            ShippingAddress = o.ShippingAddress,
            ShippingCity = o.ShippingCity,
            ShippingDistrict = o.ShippingDistrict,
            ShippingPostalCode = o.ShippingPostalCode,
            BillingFirstName = o.BillingFirstName,
            BillingLastName = o.BillingLastName,
            BillingPhone = o.BillingPhone,
            BillingAddress = o.BillingAddress,
            BillingCity = o.BillingCity,
            BillingDistrict = o.BillingDistrict,
            BillingPostalCode = o.BillingPostalCode,
            TaxNumber = o.TaxNumber,
            CompanyName = o.CompanyName,
            CustomerNote = o.CustomerNote,
            AdminNote = o.AdminNote,
            CreateTime = o.CreateTime,
            PaidAt = o.PaidAt,
            ShippedAt = o.ShippedAt,
            DeliveredAt = o.DeliveredAt,
            CancelledAt = o.CancelledAt,
            CargoCompany = o.CargoCompany,
            TrackingNumber = o.TrackingNumber,
            Items = o.Items?.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                ProductSku = i.ProductSku,
                ProductOemNumber = i.ProductOemNumber,
                ProductImageUrl = i.ProductImageUrl,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                DiscountAmount = i.DiscountAmount,
                TaxRate = i.TaxRate,
                TaxAmount = i.TaxAmount,
                TotalPrice = i.TotalPrice
            }).ToList() ?? new()
        };
    }
}
