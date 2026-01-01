using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    /// <summary>
    /// Kategori ile özellik tanımı arasındaki ilişki
    /// Hangi kategoride hangi özellikler olacağını belirler
    /// </summary>
    public class CategoryAttribute : BaseEntity
    {
        public Guid CategoryId { get; set; }
        public Guid AttributeDefinitionId { get; set; }
        public bool IsRequired { get; set; }            // Bu kategoride zorunlu mu?
        public int DisplayOrder { get; set; }

        // Navigation
        public virtual Category Category { get; set; } = null!;
    }
}
