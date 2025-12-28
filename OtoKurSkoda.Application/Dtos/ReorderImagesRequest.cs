namespace OtoKurSkoda.Application.Dtos
{
    public class ReorderImagesRequest
    {
        public Guid ProductId { get; set; }
        public List<Guid> ImageIds { get; set; } = new();
    }
}
