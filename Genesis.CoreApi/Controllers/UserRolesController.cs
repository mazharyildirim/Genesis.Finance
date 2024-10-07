using AutoMapper;
using Genesis.CoreApi.Repository;
using Genesis.CoreApi.Shared;
using Genesis.Shared;
using Genesis.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Genesis.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserRolesController : Controller
    {
        private readonly  IUserRolesRepository _repository;
        private readonly IMapper _mapper;
        public UserRolesController(IUserRolesRepository repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

       

        [HttpPost("AddUserRole")]
        public async Task<Response<NoContent>> AddUser([FromBody] UserRoleDTO userRoleDTO, CancellationToken cancellationToken)
        {
            var userRole = _mapper.Map<Genesis.Shared.Models.UserManagement.UserRoles>(userRoleDTO);
            var result = await _repository.AddUserRole(userRole.UserId, userRole.RoleId);
            if (result.IsSuccess)
                return Response<NoContent>.Success(204);
            else
            {
                return Response<NoContent>.Fail(!string.IsNullOrEmpty(result.Message) ? result.Message : string.Empty, 404);
            }
        }

        [HttpPut("DeleteByUser")]
        public async Task<Response<NoContent>> DeleteByUser([FromQuery] int userId, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteByUser(userId);
            if (result.IsSuccess)
                return Response<NoContent>.Success(204);
            else
            {
                return Response<NoContent>.Fail(!string.IsNullOrEmpty(result.Message) ? result.Message : string.Empty, 404);
            }
        }
    }
}
