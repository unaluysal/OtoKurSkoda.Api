using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    /// <summary>
    /// Araç nesli/kasası (Octavia 1, Octavia 2 (1Z), Octavia 3 (5E) vb.)
    /// </summary>
    public class VehicleGeneration : BaseEntity
    {
        public Guid VehicleModelId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }           // Kasa kodu: "5E", "1Z", "NE"
        public int StartYear { get; set; }
        public int? EndYear { get; set; }           // null = hala üretimde
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        // Navigation
        public virtual VehicleModel VehicleModel { get; set; } = null!;
        public virtual ICollection<ProductCompatibility> CompatibleProducts { get; set; } = new List<ProductCompatibility>();
    }
}
