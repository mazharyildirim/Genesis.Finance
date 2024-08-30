using AutoMapper;
using Genesis.Shared.DTO;

namespace Genesis.CoreApi.Mapping
{
    public class GeneralMapping: Profile
    {
        public GeneralMapping()
        {
            CreateMap<Genesis.Shared.Models.UserManagement.Roles, RoleDTO>().ReverseMap();
            CreateMap<RoleDTO, Genesis.Shared.Models.UserManagement.Roles>().ReverseMap();
            CreateMap<Genesis.Shared.Models.UserManagement.Users, UserDTO>().ReverseMap();
            CreateMap<UserDTO, Genesis.Shared.Models.UserManagement.Users>().ReverseMap();
            CreateMap<UserRoleDTO, Genesis.Shared.Models.UserManagement.UserRoles>().ReverseMap();
            CreateMap<Genesis.Shared.Models.UserManagement.UserRoles, UserRoleDTO>().ReverseMap();
        }
    }
}
