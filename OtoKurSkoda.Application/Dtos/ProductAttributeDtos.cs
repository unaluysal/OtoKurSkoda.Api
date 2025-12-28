namespace OtoKurSkoda.Application.Dtos
{
    public class ProductAttributeDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid AttributeDefinitionId { get; set; }
        public string AttributeName { get; set; } = string.Empty;
        public string? Unit { get; set; }
        public string Value { get; set; } = string.Empty;
    }

    public class CreateProductAttributeRequest
    {
        public Guid AttributeDefinitionId { get; set; }
        public string Value { get; set; } = string.Empty;
    }

    public class UpdateProductAttributeRequest
    {
        public Guid Id { get; set; }
        public string Value { get; set; } = string.Empty;
    }

    public class SetProductAttributesRequest
    {
        public Guid ProductId { get; set; }
        public List<CreateProductAttributeRequest> Attributes { get; set; } = new();
    }
}
