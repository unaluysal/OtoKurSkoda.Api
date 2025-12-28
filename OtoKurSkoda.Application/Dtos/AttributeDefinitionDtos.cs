namespace OtoKurSkoda.Application.Dtos
{
    public class AttributeDefinitionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string DataType { get; set; } = "text";
        public string? Unit { get; set; }
        public List<string> PossibleValues { get; set; } = new();
        public string? DefaultValue { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsRequired { get; set; }
        public bool IsVisibleOnProductPage { get; set; }
        public int DisplayOrder { get; set; }
        public bool Status { get; set; }
    }

    public class AttributeDefinitionListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string? Unit { get; set; }
        public bool IsFilterable { get; set; }
    }

    public class CreateAttributeDefinitionRequest
    {
        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; } = "text";
        public string? Unit { get; set; }
        public List<string>? PossibleValues { get; set; }
        public string? DefaultValue { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsRequired { get; set; }
        public bool IsVisibleOnProductPage { get; set; } = true;
        public int DisplayOrder { get; set; }
    }

    public class UpdateAttributeDefinitionRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; } = "text";
        public string? Unit { get; set; }
        public List<string>? PossibleValues { get; set; }
        public string? DefaultValue { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsRequired { get; set; }
        public bool IsVisibleOnProductPage { get; set; }
        public int DisplayOrder { get; set; }
    }
}
