using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    /// <summary>
    /// Araç modeli (Octavia, Fabia, Superb, Golf, Passat vb.)
    /// </summary>
    public class VehicleModel : BaseEntity
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }

        // Navigation
        public virtual Brand Brand { get; set; } = null!;
        public virtual ICollection<VehicleGeneration> Generations { get; set; } = new List<VehicleGeneration>();
    }
}
