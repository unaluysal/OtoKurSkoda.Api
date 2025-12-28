using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneConfirmed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? LastLoginAt { get; set; }  

        // Navigation Properties 
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserAddress> Addresses { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
