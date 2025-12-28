namespace OtoKurSkoda.Application.Dtos
{
    public class VehicleGenerationDto
    {
        public Guid Id { get; set; }
        public Guid VehicleModelId { get; set; }
        public string VehicleModelName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public string YearRange => EndYear.HasValue ? $"{StartYear}-{EndYear}" : $"{StartYear}-";
    }

    public class VehicleGenerationListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class CreateVehicleGenerationRequest
    {
        public Guid VehicleModelId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateVehicleGenerationRequest
    {
        public Guid Id { get; set; }
        public Guid VehicleModelId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }
}
