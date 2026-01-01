using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    public class CartItem : BaseEntity
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime AddedAt { get; set; }

        // Navigation Properties
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
}
