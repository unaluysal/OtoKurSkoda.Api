using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    /// <summary>
    /// Ürün görselleri
    /// </summary>
    public class ProductImage : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public string? AltText { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsMain { get; set; }

        // Navigation
        public virtual Product Product { get; set; } = null!;
    }
}
