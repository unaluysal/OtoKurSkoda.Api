namespace OtoKurSkoda.Application.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string? ParentName { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? IconClass { get; set; }
        public string? Color { get; set; }
        public int DisplayOrder { get; set; }
        public int Level { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        public bool Status { get; set; }
        public int ChildCount { get; set; }
        public int ProductCount { get; set; }
    }

    public class CategoryTreeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? IconClass { get; set; }
        public string? Color { get; set; }
        public int DisplayOrder { get; set; }
        public int Level { get; set; }
        public int ProductCount { get; set; }
        public List<CategoryTreeDto> Children { get; set; } = new();
    }

    public class CategoryListDto
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string? ParentName { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Color { get; set; }
        public int Level { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreateCategoryRequest
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? IconClass { get; set; }
        public string? Color { get; set; }
        public int DisplayOrder { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
    }

    public class UpdateCategoryRequest
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? IconClass { get; set; }
        public string? Color { get; set; }
        public int DisplayOrder { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
    }
}
