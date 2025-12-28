namespace OtoKurSkoda.Application.Dtos
{
    public class AssignAttributeToCategoryRequest
    {
        public Guid CategoryId { get; set; }
        public Guid AttributeId { get; set; }
        public bool IsRequired { get; set; }
    }
}
