namespace OtoKurSkoda.Application.Dtos
{
    // Cart DTOs
    public class CartDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public decimal SubTotal { get; set; }
        public int TotalItems { get; set; }
        public DateTime LastActivityAt { get; set; }
    }

    public class CartItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSlug { get; set; }
        public string ProductSku { get; set; }
        public string ProductOemNumber { get; set; }
        public string ProductImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsInStock { get; set; }
    }

    // Request DTOs
    public class AddToCartRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartItemRequest
    {
        public Guid CartItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class RemoveFromCartRequest
    {
        public Guid CartItemId { get; set; }
    }
}
