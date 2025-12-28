using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    /// <summary>
    /// Ürün (Yağ Filtresi, Hava Filtresi, Fren Balatası vb.)
    /// </summary>
    public class Product : BaseEntity
    {
        public Guid CategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }

        // Temel Bilgiler
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }        // HTML içerik destekli
        public string Sku { get; set; } = string.Empty; // Stok kodu
        public string? Barcode { get; set; }
        public string? OemNumber { get; set; }          // Orijinal parça numarası

        // Fiyat
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public decimal? CostPrice { get; set; }         // Maliyet (admin için)
        public decimal? TaxRate { get; set; }           // KDV oranı

        // Stok
        public int StockQuantity { get; set; }
        public int? LowStockThreshold { get; set; }     // Düşük stok uyarı sınırı
        public bool TrackInventory { get; set; } = true;

        // Durum Flagleri
        public bool IsFeatured { get; set; }            // Öne çıkan
        public bool IsNewArrival { get; set; }          // Yeni ürün
        public bool IsBestSeller { get; set; }          // Çok satan
        public bool IsOnSale { get; set; }              // İndirimde

        // SEO
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        // İstatistik
        public int ViewCount { get; set; }
        public int SoldCount { get; set; }

        // Navigation
        public virtual Category Category { get; set; } = null!;
        public virtual Manufacturer? Manufacturer { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public virtual ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
        public virtual ICollection<ProductCompatibility> Compatibilities { get; set; } = new List<ProductCompatibility>();
        public virtual ICollection<RelatedProduct> RelatedProducts { get; set; } = new List<RelatedProduct>();
        public virtual ICollection<RelatedProduct> RelatedToProducts { get; set; } = new List<RelatedProduct>();
    }
}
