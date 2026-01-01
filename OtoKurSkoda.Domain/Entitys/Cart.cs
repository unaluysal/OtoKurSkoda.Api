using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime LastActivityAt { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual ICollection<CartItem> Items { get; set; }
    }
}
