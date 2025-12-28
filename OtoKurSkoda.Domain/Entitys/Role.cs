using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    public class Role : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

        public virtual ICollection<RoleGroupRole> RoleGroupRoles { get; set; }

    }
}
