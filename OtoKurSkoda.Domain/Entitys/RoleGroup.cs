using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    public class RoleGroup : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<RoleGroupRole> RoleGroupRoles { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
