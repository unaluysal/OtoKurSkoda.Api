namespace OtoKurSkoda.Application.Dtos
{
    public class ProductSearchRequest
    {
        public string SearchTerm { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
