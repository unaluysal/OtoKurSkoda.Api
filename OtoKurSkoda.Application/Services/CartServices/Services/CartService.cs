using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.CartServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;

namespace OtoKurSkoda.Application.Services.CartServices.Services
{
    public class CartService : BaseApplicationService, ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> GetCartAsync(Guid userId)
        {
            var cartRepo = _unitOfWork.GetRepository<Cart>();

            var cart = await cartRepo.GetWhere(x => x.UserId == userId && x.Status)
                .Include(x => x.Items.Where(i => i.Status))
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images.Where(img => img.IsMain && img.Status))
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                // Sepet yoksa boş sepet döndür
                return SuccessDataResult(new CartDto
                {
                    UserId = userId,
                    Items = new List<CartItemDto>(),
                    SubTotal = 0,
                    TotalItems = 0
                }, "CART_EMPTY", "Sepetiniz boş.");
            }

            var cartDto = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                LastActivityAt = cart.LastActivityAt,
                Items = cart.Items.Select(i => new CartItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    ProductSlug = i.Product.Slug,
                    ProductSku = i.Product.Sku,
                    ProductOemNumber = i.Product.OemNumber,
                    ProductImageUrl = i.Product.Images.FirstOrDefault()?.Url,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product.DiscountedPrice ?? i.Product.Price,
                    TotalPrice = (i.Product.DiscountedPrice ?? i.Product.Price) * i.Quantity,
                    StockQuantity = i.Product.StockQuantity,
                    IsInStock = i.Product.StockQuantity >= i.Quantity
                }).ToList()
            };

            cartDto.SubTotal = cartDto.Items.Sum(i => i.TotalPrice);
            cartDto.TotalItems = cartDto.Items.Sum(i => i.Quantity);

            return SuccessDataResult(cartDto, "CART_FOUND", "Sepet bulundu.");
        }

        public async Task<ServiceResult> AddToCartAsync(Guid userId, Guid productId, int quantity)
        {
            if (quantity <= 0)
                return ErrorResult("INVALID_QUANTITY", "Miktar 0'dan büyük olmalıdır.");

            var productRepo = _unitOfWork.GetRepository<Product>();
            var product = await productRepo.GetWhere(x => x.Id == productId && x.Status).FirstOrDefaultAsync();

            if (product == null)
                return ErrorResult("PRODUCT_NOT_FOUND", "Ürün bulunamadı.");

            if (product.StockQuantity < quantity)
                return ErrorResult("INSUFFICIENT_STOCK", $"Yetersiz stok. Mevcut stok: {product.StockQuantity}");

            var cartRepo = _unitOfWork.GetRepository<Cart>();
            var cartItemRepo = _unitOfWork.GetRepository<CartItem>();

            // Kullanıcının sepetini bul veya oluştur
            var cart = await cartRepo.GetWhere(x => x.UserId == userId && x.Status)
                .Include(x => x.Items.Where(i => i.Status))
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    LastActivityAt = DateTime.UtcNow
                };
                await cartRepo.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync();
            }

            // Ürün sepette var mı kontrol et
            var existingItem = cart.Items?.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                // Miktarı güncelle
                var newQuantity = existingItem.Quantity + quantity;
                if (product.StockQuantity < newQuantity)
                    return ErrorResult("INSUFFICIENT_STOCK", $"Yetersiz stok. Mevcut stok: {product.StockQuantity}");

                existingItem.Quantity = newQuantity;
                existingItem.UnitPrice = product.DiscountedPrice ?? product.Price;
                cartItemRepo.Update(existingItem);
            }
            else
            {
                // Yeni ürün ekle
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.DiscountedPrice ?? product.Price,
                    AddedAt = DateTime.UtcNow
                };
                await cartItemRepo.AddAsync(cartItem);
            }

            cart.LastActivityAt = DateTime.UtcNow;
            cartRepo.Update(cart);
            await _unitOfWork.SaveChangesAsync();

            return await GetCartAsync(userId);
        }

        public async Task<ServiceResult> UpdateQuantityAsync(Guid userId, Guid cartItemId, int quantity)
        {
            if (quantity <= 0)
                return await RemoveFromCartAsync(userId, cartItemId);

            var cartItemRepo = _unitOfWork.GetRepository<CartItem>();
            var cartItem = await cartItemRepo.GetWhere(x => x.Id == cartItemId && x.Status)
                .Include(x => x.Cart)
                .Include(x => x.Product)
                .FirstOrDefaultAsync();

            if (cartItem == null)
                return ErrorResult("CART_ITEM_NOT_FOUND", "Sepet ürünü bulunamadı.");

            if (cartItem.Cart.UserId != userId)
                return ErrorResult("UNAUTHORIZED", "Bu işlem için yetkiniz yok.");

            if (cartItem.Product.StockQuantity < quantity)
                return ErrorResult("INSUFFICIENT_STOCK", $"Yetersiz stok. Mevcut stok: {cartItem.Product.StockQuantity}");

            cartItem.Quantity = quantity;
            cartItem.UnitPrice = cartItem.Product.DiscountedPrice ?? cartItem.Product.Price;
            cartItemRepo.Update(cartItem);

            cartItem.Cart.LastActivityAt = DateTime.UtcNow;
            _unitOfWork.GetRepository<Cart>().Update(cartItem.Cart);

            await _unitOfWork.SaveChangesAsync();

            return await GetCartAsync(userId);
        }

        public async Task<ServiceResult> RemoveFromCartAsync(Guid userId, Guid cartItemId)
        {
            var cartItemRepo = _unitOfWork.GetRepository<CartItem>();
            var cartItem = await cartItemRepo.GetWhere(x => x.Id == cartItemId && x.Status)
                .Include(x => x.Cart)
                .FirstOrDefaultAsync();

            if (cartItem == null)
                return ErrorResult("CART_ITEM_NOT_FOUND", "Sepet ürünü bulunamadı.");

            if (cartItem.Cart.UserId != userId)
                return ErrorResult("UNAUTHORIZED", "Bu işlem için yetkiniz yok.");

            cartItem.Status = false;
            cartItemRepo.Update(cartItem);

            cartItem.Cart.LastActivityAt = DateTime.UtcNow;
            _unitOfWork.GetRepository<Cart>().Update(cartItem.Cart);

            await _unitOfWork.SaveChangesAsync();

            return await GetCartAsync(userId);
        }

        public async Task<ServiceResult> ClearCartAsync(Guid userId)
        {
            var cartRepo = _unitOfWork.GetRepository<Cart>();
            var cart = await cartRepo.GetWhere(x => x.UserId == userId && x.Status)
                .Include(x => x.Items.Where(i => i.Status))
                .FirstOrDefaultAsync();

            if (cart == null)
                return SuccessResult("CART_CLEARED", "Sepet zaten boş.");

            var cartItemRepo = _unitOfWork.GetRepository<CartItem>();
            foreach (var item in cart.Items)
            {
                item.Status = false;
                cartItemRepo.Update(item);
            }

            cart.LastActivityAt = DateTime.UtcNow;
            cartRepo.Update(cart);

            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("CART_CLEARED", "Sepet temizlendi.");
        }

        public async Task<ServiceResult> GetCartItemCountAsync(Guid userId)
        {
            var cartRepo = _unitOfWork.GetRepository<Cart>();
            var cart = await cartRepo.GetWhere(x => x.UserId == userId && x.Status)
                .Include(x => x.Items.Where(i => i.Status))
                .FirstOrDefaultAsync();

            var count = cart?.Items?.Sum(i => i.Quantity) ?? 0;

            return SuccessDataResult(new { Count = count }, "CART_COUNT", "Sepet sayısı.");
        }
    }
}
