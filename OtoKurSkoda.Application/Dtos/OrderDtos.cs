using OtoKurSkoda.Domain.Entitys;

namespace OtoKurSkoda.Application.Dtos
{
    // Order DTOs
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string OrderStatusText { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusText { get; set; }

        // Fiyatlar
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }

        // Teslimat Bilgileri
        public string ShippingFirstName { get; set; }
        public string ShippingLastName { get; set; }
        public string ShippingFullName => $"{ShippingFirstName} {ShippingLastName}";
        public string ShippingPhone { get; set; }
        public string ShippingEmail { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingDistrict { get; set; }
        public string ShippingPostalCode { get; set; }
        public string ShippingFullAddress => $"{ShippingAddress}, {ShippingDistrict}/{ShippingCity}";

        // Fatura Bilgileri
        public string BillingFirstName { get; set; }
        public string BillingLastName { get; set; }
        public string BillingPhone { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingDistrict { get; set; }
        public string BillingPostalCode { get; set; }
        public string TaxNumber { get; set; }
        public string CompanyName { get; set; }

        // Notlar
        public string CustomerNote { get; set; }
        public string AdminNote { get; set; }

        // Tarihler
        public DateTime CreateTime { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        // Kargo
        public string CargoCompany { get; set; }
        public string TrackingNumber { get; set; }

        // Items
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderListDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string OrderStatusText { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusText { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string ProductOemNumber { get; set; }
        public string ProductImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalPrice { get; set; }
    }

    // Request DTOs
    public class CreateOrderRequest
    {
        // Teslimat Bilgileri
        public string ShippingFirstName { get; set; }
        public string ShippingLastName { get; set; }
        public string ShippingPhone { get; set; }
        public string ShippingEmail { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingDistrict { get; set; }
        public string ShippingPostalCode { get; set; }

        // Fatura Bilgileri (Opsiyonel - boşsa teslimat adresi kullanılır)
        public bool UseSameAddressForBilling { get; set; } = true;
        public string BillingFirstName { get; set; }
        public string BillingLastName { get; set; }
        public string BillingPhone { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingDistrict { get; set; }
        public string BillingPostalCode { get; set; }
        public string TaxNumber { get; set; }
        public string CompanyName { get; set; }

        // Not
        public string CustomerNote { get; set; }
    }

    public class UpdateOrderStatusRequest
    {
        public Guid OrderId { get; set; }
        public OrderStatus NewStatus { get; set; }
        public string AdminNote { get; set; }
    }

    public class UpdateShippingInfoRequest
    {
        public Guid OrderId { get; set; }
        public string CargoCompany { get; set; }
        public string TrackingNumber { get; set; }
    }

    public class OrderFilterRequest
    {
        public Guid? UserId { get; set; }
        public OrderStatus? Status { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
