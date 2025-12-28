namespace OtoKurSkoda.Application.Dtos
{
    public class ProductByManufacturerRequest
    {
        public Guid ManufacturerId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
