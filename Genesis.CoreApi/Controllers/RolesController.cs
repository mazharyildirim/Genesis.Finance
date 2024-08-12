using AutoMapper;
using Genesis.Core.Models;
using Genesis.CoreApi.DTO;
using Genesis.CoreApi.Repository;
using Genesis.CoreApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Genesis.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : Controller
    {
        private readonly IRolesRepository _repository;
        private readonly IMapper _mapper;
        public RolesController(IRolesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("GetAllRoles")]
        public Response<List<RoleDTO>> GetAllRoles(CancellationToken cancellationToken)
        {
            var roles =  _repository.GetAll().ToList();
            
            return Response<List<RoleDTO>>.Success(_mapper.Map<List<RoleDTO>>(roles), 200);
        }

        [HttpGet("GetRoleId")]
        public async Task<Response<RoleDTO>> GetRoleId([FromQuery] int roleId, CancellationToken cancellationToken)
        {
            var role = await _repository.GetId(roleId);

            return Response<RoleDTO>.Success(_mapper.Map<RoleDTO>(role), 200);
        }

        [HttpPost("AddRole")]
        public async Task<Response<RoleDTO>> AddRole([FromBody] RoleDTO  roleDTO, CancellationToken cancellationToken)
        {
            var newRole = _mapper.Map<Roles>(roleDTO);
            var role = await _repository.Create(newRole);
            
            return Response<RoleDTO>.Success(_mapper.Map<RoleDTO>(role.ResultData), 200);
        }

        [HttpPut("UpdateRole")]
        public async Task<Response<NoContent>> UpdateRole([FromBody] RoleDTO roleDTO, CancellationToken cancellationToken)
        {
            var updateRole = _mapper.Map<Roles>(roleDTO);
            var result = await _repository.Update(updateRole);
            if (result.IsSuccess)
                return Response<NoContent>.Success(204);
            else
            {
                return Response<NoContent>.Fail(!string.IsNullOrEmpty(result.Message) ? result.Message : string.Empty, 404);
            }
                
        }

        [HttpPut("DeleteRole")]
        public async Task<Response<NoContent>> DeleteRole([FromBody] int roleId, CancellationToken cancellationToken)
        {
            var result = await _repository.Delete(roleId);
            if (result.IsSuccess)
                return Response<NoContent>.Success(204);
            else
            {
                return Response<NoContent>.Fail(!string.IsNullOrEmpty(result.Message) ? result.Message : string.Empty, 404);
            }
        }
    }
}
