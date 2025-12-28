using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    /// <summary>
    /// Ürün özellik tanımları (Malzeme, Çap, Uzunluk, Renk vb.)
    /// EAV (Entity-Attribute-Value) pattern ile dinamik özellik desteği
    /// </summary>
    public class AttributeDefinition : BaseEntity
    {
        public string Name { get; set; } = string.Empty;        // "Malzeme", "Çap", "Uzunluk"
        public string Slug { get; set; } = string.Empty;
        public string DataType { get; set; } = "text";          // "text", "number", "select", "boolean"
        public string? Unit { get; set; }                       // "mm", "kg", "cm", "adet"
        public string? PossibleValues { get; set; }             // JSON: ["Plastik","Metal","Kauçuk"]
        public string? DefaultValue { get; set; }
        public bool IsFilterable { get; set; }                  // Filtre olarak kullanılsın mı?
        public bool IsRequired { get; set; }
        public bool IsVisibleOnProductPage { get; set; } = true;
        public int DisplayOrder { get; set; }

        // Navigation
        public virtual ICollection<CategoryAttribute> CategoryAttributes { get; set; } = new List<CategoryAttribute>();
        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();
    }
}
