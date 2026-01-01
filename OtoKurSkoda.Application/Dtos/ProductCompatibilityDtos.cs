namespace OtoKurSkoda.Application.Dtos
{
    public class ProductCompatibilityDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid VehicleGenerationId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string VehicleModelName { get; set; } = string.Empty;
        public string VehicleGenerationName { get; set; } = string.Empty;
        public string? Code { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
        public string? Notes { get; set; }
        public string FullName => $"{BrandName} {VehicleModelName} {VehicleGenerationName}";
    }

    public class AddProductCompatibilityRequest
    {
        public Guid ProductId { get; set; }
        public Guid VehicleGenerationId { get; set; }
        public string? Notes { get; set; }
    }

    public class SetProductCompatibilitiesRequest
    {
        public Guid ProductId { get; set; }
        public List<Guid> VehicleGenerationIds { get; set; } = new();
    }

}
