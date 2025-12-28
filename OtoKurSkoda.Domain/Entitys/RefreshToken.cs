using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

        public string? CreatedByIp { get; set; }
        public string? UserAgent { get; set; }

        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }  // Yenilendiğinde yeni token

        // Hesaplanan property
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
