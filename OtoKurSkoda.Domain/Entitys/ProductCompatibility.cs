using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    /// <summary>
    /// Ürün ile araç nesli arasındaki uyumluluk ilişkisi
    /// Hangi ürün hangi araçlara uyuyor
    /// </summary>
    public class ProductCompatibility : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid VehicleGenerationId { get; set; }
        public string? Notes { get; set; }              // "Sadece dizel motorlar", "1.6 TDI hariç" vb.

        // Navigation
        public virtual Product Product { get; set; } = null!;
        public virtual VehicleGeneration VehicleGeneration { get; set; } = null!;
    }
}
