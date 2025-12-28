using AutoMapper;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Domain.Entitys;

namespace OtoKurSkoda.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                    src.UserRoles != null
                        ? src.UserRoles.Select(ur => ur.RoleGroup.Name).ToList()
                        : new List<string>()));

            // Role
            CreateMap<Role, RoleDto>();

            // RoleGroup
            CreateMap<RoleGroup, RoleGroupDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());

            // RoleGroupRole
            CreateMap<RoleGroupRole, RoleGroupRoleDto>()
                .ForMember(dest => dest.RoleGroupDto, opt => opt.MapFrom(src => src.RoleGroup))
                .ForMember(dest => dest.RoleDto, opt => opt.MapFrom(src => src.Role));

            // UserRole
            CreateMap<UserRole, UserRoleDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.RoleGroup, opt => opt.MapFrom(src => src.RoleGroup));
        }
    }
}