using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    /// <summary>
    /// Araç markası (Skoda, Volkswagen, Audi, Seat vb.)
    /// </summary>
    public class Brand : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }

        // Navigation
        public virtual ICollection<VehicleModel> VehicleModels { get; set; } = new List<VehicleModel>();
    }
}
