namespace OtoKurSkoda.Application.Dtos
{
    public class ProductImageDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public string? AltText { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsMain { get; set; }
    }

    public class CreateProductImageRequest
    {
        public string Url { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public string? AltText { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsMain { get; set; }
    }

    public class UpdateProductImageRequest
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public string? AltText { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsMain { get; set; }
    }

    public class AddProductImageRequest
    {
        public Guid ProductId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public string? AltText { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsMain { get; set; }
    }
}
