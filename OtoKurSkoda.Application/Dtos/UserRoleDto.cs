using OtoKurSkoda.Application.Defaults;

namespace OtoKurSkoda.Application.Dtos
{
    public class UserRoleDto : BaseDto
    {
        public Guid UserId { get; set; }
        public UserDto? User { get; set; }
        public Guid RoleGroupId { get; set; }
        public RoleGroupDto? RoleGroup { get; set; }
    }
}
