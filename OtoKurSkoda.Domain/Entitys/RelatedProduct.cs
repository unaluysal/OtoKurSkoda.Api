namespace OtoKurSkoda.Domain.Entitys
{
    /// <summary>
    /// İlişkili ürünler (Benzer ürünler, aksesuarlar, alternatifler)
    /// </summary>
    public class RelatedProduct
    {
        public Guid ProductId { get; set; }
        public Guid RelatedProductId { get; set; }
        public string RelationType { get; set; } = "similar";  // "similar", "accessory", "replacement", "upsell", "crosssell"
        public int DisplayOrder { get; set; }

        // Navigation
        public virtual Product Product { get; set; } = null!;
        public virtual Product Related { get; set; } = null!;
    }
}
