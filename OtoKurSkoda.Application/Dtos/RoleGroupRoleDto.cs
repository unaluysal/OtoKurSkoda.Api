using OtoKurSkoda.Application.Defaults;

namespace OtoKurSkoda.Application.Dtos
{
    public class RoleGroupRoleDto : BaseDto
    {
        public Guid RoleGroupId { get; set; }
        public virtual RoleGroupDto? RoleGroupDto { get; set; }
        public Guid RoleId { get; set; }
        public virtual RoleDto? RoleDto { get; set; }
    }
}
