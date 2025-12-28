namespace OtoKurSkoda.Application.Dtos
{
    public class ProductByVehicleRequest
    {
        public Guid VehicleGenerationId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
