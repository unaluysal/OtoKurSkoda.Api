namespace OtoKurSkoda.Application.Dtos
{
    public class UpdateStockRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
