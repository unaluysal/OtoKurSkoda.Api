namespace OtoKurSkoda.Application.Dtos
{
    public class ManufacturerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool Status { get; set; }
        public int ProductCount { get; set; }
    }

    public class ManufacturerListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
    }

    public class CreateManufacturerRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class UpdateManufacturerRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
