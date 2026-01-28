using AutoMapper;
using GRC.Identity.Application.DTOs;
using GRC.Identity.Domain.Aggregates.UserAggregate;
using GRC.Identity.Domain.Aggregates.RoleAggregate;

namespace GRC.Identity.Application.Mappings;

public class IdentityMappingProfile : Profile
{
    public IdentityMappingProfile()
    {        
        CreateMap<User, UserDto>()
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
             .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                 src.UserRoles.Select(ur => new RoleDto
                 {
                     Id = ur.RoleId,                     
                 })));

        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.PermissionCount, opt => opt.MapFrom(src => src.Permissions.Count));

        CreateMap<Role, RoleWithPermissionsDto>()
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions));

        CreateMap<Permission, PermissionDto>();
    }
}