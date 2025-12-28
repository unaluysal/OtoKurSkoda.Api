namespace OtoKurSkoda.Application.Dtos
{
    public class RemoveAttributeFromCategoryRequest
    {
        public Guid CategoryId { get; set; }
        public Guid AttributeId { get; set; }
    }
}
