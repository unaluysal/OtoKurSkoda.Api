namespace OtoKurSkoda.Application.Dtos
{
    public class ProductByCategoryRequest
    {
        public Guid CategoryId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
