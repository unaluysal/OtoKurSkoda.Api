namespace OtoKurSkoda.Application.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public Guid? ManufacturerId { get; set; }
        public string? ManufacturerName { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string? Barcode { get; set; }
        public string? OemNumber { get; set; }

        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public decimal? TaxRate { get; set; }

        public int StockQuantity { get; set; }
        public int? LowStockThreshold { get; set; }
        public bool TrackInventory { get; set; }

        public bool IsFeatured { get; set; }
        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsOnSale { get; set; }
        public bool Status { get; set; }

        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        public int ViewCount { get; set; }
        public int SoldCount { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public string? MainImageUrl { get; set; }
        public List<ProductImageDto> Images { get; set; } = new();
        public List<ProductAttributeDto> Attributes { get; set; } = new();
        public List<ProductCompatibilityDto> Compatibilities { get; set; } = new();
    }

    public class ProductListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string? OemNumber { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? ManufacturerName { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsOnSale { get; set; }
        public bool Status { get; set; }
        public string? MainImageUrl { get; set; }
    }

    public class ProductCardDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public string? MainImageUrl { get; set; }
        public string? ManufacturerName { get; set; }
        public bool IsOnSale { get; set; }
        public bool IsNewArrival { get; set; }
        public int StockQuantity { get; set; }
    }

    public class CreateProductRequest
    {
        public Guid CategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string? Barcode { get; set; }
        public string? OemNumber { get; set; }

        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? TaxRate { get; set; }

        public int StockQuantity { get; set; }
        public int? LowStockThreshold { get; set; }
        public bool TrackInventory { get; set; } = true;

        public bool IsFeatured { get; set; }
        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsOnSale { get; set; }

        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        public List<CreateProductImageRequest> Images { get; set; } = new();
        public List<CreateProductAttributeRequest> Attributes { get; set; } = new();
        public List<Guid> CompatibleVehicleGenerationIds { get; set; } = new();
    }

    public class UpdateProductRequest
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string? Barcode { get; set; }
        public string? OemNumber { get; set; }

        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? TaxRate { get; set; }

        public int StockQuantity { get; set; }
        public int? LowStockThreshold { get; set; }
        public bool TrackInventory { get; set; }

        public bool IsFeatured { get; set; }
        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsOnSale { get; set; }

        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
    }

    public class ProductFilterRequest
    {
        public string? SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? VehicleModelId { get; set; }
        public Guid? VehicleGenerationId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? InStock { get; set; }
        public bool? IsFeatured { get; set; }
        public bool? IsOnSale { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
