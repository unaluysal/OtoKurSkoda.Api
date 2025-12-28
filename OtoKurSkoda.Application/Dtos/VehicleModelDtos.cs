namespace OtoKurSkoda.Application.Dtos
{
    public class VehicleModelDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool Status { get; set; }
        public int GenerationCount { get; set; }
    }

    public class VehicleModelListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreateVehicleModelRequest
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class UpdateVehicleModelRequest
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
