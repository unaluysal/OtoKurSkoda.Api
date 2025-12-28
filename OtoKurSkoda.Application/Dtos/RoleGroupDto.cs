using OtoKurSkoda.Application.Defaults;

namespace OtoKurSkoda.Application.Dtos
{
    public class RoleGroupDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RoleDto> Roles { get; set; } = new();
    }
}
