using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        // Fiyatlar
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }

        // Teslimat Bilgileri
        public string ShippingFirstName { get; set; }
        public string ShippingLastName { get; set; }
        public string ShippingPhone { get; set; }
        public string ShippingEmail { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingDistrict { get; set; }
        public string ShippingPostalCode { get; set; }

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
        public DateTime? PaidAt { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        // Kargo Bilgileri
        public string CargoCompany { get; set; }
        public string TrackingNumber { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }
    }

    public enum OrderStatus
    {
        Pending = 0,        // Beklemede
        Confirmed = 1,      // Onaylandı
        Processing = 2,     // Hazırlanıyor
        Shipped = 3,        // Kargoya Verildi
        Delivered = 4,      // Teslim Edildi
        Cancelled = 5,      // İptal Edildi
        Returned = 6        // İade Edildi
    }

    public enum PaymentStatus
    {
        Pending = 0,        // Ödeme Bekleniyor
        Paid = 1,           // Ödendi
        Failed = 2,         // Ödeme Başarısız
        Refunded = 3,       // İade Edildi
        PartialRefund = 4   // Kısmi İade
    }
}
