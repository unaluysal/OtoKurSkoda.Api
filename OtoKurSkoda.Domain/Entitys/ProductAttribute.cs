using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    /// <summary>
    /// Ürünün özellikleri (EAV pattern - value kısmı)
    /// </summary>
    public class ProductAttribute : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid AttributeDefinitionId { get; set; }
        public string Value { get; set; } = string.Empty;

        // Navigation
        public virtual Product Product { get; set; } = null!;
        public virtual AttributeDefinition AttributeDefinition { get; set; } = null!;
    }
}
