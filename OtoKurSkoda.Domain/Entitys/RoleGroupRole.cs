using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    public class RoleGroupRole : BaseEntity
    {
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
        public Guid RoleGroupId { get; set; }
        public virtual RoleGroup RoleGroup { get; set; }

    }
}
