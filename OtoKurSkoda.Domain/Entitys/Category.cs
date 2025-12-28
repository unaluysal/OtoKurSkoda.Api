using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    /// <summary>
    /// Ürün kategorisi (Motor Parçaları, Kaporta, Elektrik, Fren Sistemi vb.)
    /// Self-referencing yapı ile sınırsız alt kategori desteği
    /// </summary>
    public class Category : BaseEntity
    {
        public Guid? ParentId { get; set; }         // null = ana kategori
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? IconClass { get; set; }      // Font Awesome veya benzeri ikon
        public int DisplayOrder { get; set; }
        public int Level { get; set; }              // 0 = ana, 1 = alt, 2 = alt-alt...

        // SEO
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        // Navigation
        public virtual Category? Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; } = new List<Category>();
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<CategoryAttribute> CategoryAttributes { get; set; } = new List<CategoryAttribute>();
    }
}
