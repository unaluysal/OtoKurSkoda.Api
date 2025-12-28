using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    public class UserRole : BaseEntity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid RoleGroupId { get; set; }
        public virtual RoleGroup RoleGroup { get; set; }
    }
}
