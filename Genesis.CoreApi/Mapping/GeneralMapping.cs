using AutoMapper;
using AutoMapper.Features;
using Genesis.Core.Models;
using Genesis.CoreApi.DTO;

namespace Genesis.CoreApi.Mapping
{
    public class GeneralMapping: Profile
    {
        public GeneralMapping()
        {
            CreateMap<Roles, RoleDTO>().ReverseMap();
            CreateMap<RoleDTO,Roles>().ReverseMap();
            CreateMap<Users, UserDTO>().ReverseMap();
            CreateMap<UserDTO, Users>().ReverseMap();
            CreateMap<UserRoleDTO, UserRoles>().ReverseMap();
            CreateMap< UserRoles, UserRoleDTO>().ReverseMap();
        }
    }
}
