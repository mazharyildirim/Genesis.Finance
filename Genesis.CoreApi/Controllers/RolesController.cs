using AutoMapper;
using Genesis.CoreApi.Repository;
using Genesis.CoreApi.Shared;
using Genesis.Shared;
using Genesis.Shared.DTO;
using Genesis.Shared.UserRoleManagements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Genesis.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : Controller
    {
        private readonly IRolesRepository _repository;
        private readonly IMapper _mapper;
        private CoreDBContext _context;
        public RolesController(IRolesRepository repository, IMapper mapper,  CoreDBContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles(
             int pageIndex = 0,
             int pageSize = 10,
             string? sortColumn = null,
             string? sortOrder = null,
             string? filterColumn = null,
             string? filterQuery = null)
        {
            var data = await ApiResult<RoleListModel>.CreateAsync(
                     _context.Roles.Where(r => r.IsDeleted == 0 && r.IsActive == 1).AsNoTracking()
                         .Select(c => new RoleListModel()
                         {
                             RoleId = c.RoleId,
                             RoleName = c.RoleName
                         }),
                     pageIndex,
                     pageSize,
                     sortColumn,
                     sortOrder,
                     filterColumn,
                     filterQuery);
            RoleResponse response = new RoleResponse();
            response.List = data.Data;
            response.PageIndex = data.PageIndex;
            response.PageSize = data.PageSize;
            response.TotalCount = data.TotalCount;
            response.TotalPages = data.TotalPages;
            return Ok(response);
        }


        [HttpGet("GetAllRoles")]
        public Response<List<RoleDTO>> GetAllRoles(CancellationToken cancellationToken)
        {
            var roles =  _repository.GetAll(cancellationToken);
            
            return Response<List<RoleDTO>>.Success(_mapper.Map<List<RoleDTO>>(roles), 200);
        }

        [HttpGet("GetRoleId")]
        public  IActionResult GetRoleId([FromQuery] int roleId, CancellationToken cancellationToken)
        {
            var role = _repository.GetId(roleId, cancellationToken);
            var data = _mapper.Map<RoleDTO>(role.Result.ResultData);

            return Ok(data);
        }



        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] RoleDTO  roleDTO, CancellationToken cancellationToken)
        {
            try
            {
                var newRole = _mapper.Map<Genesis.Shared.Models.UserManagement.Roles>(roleDTO);
                var result = await _repository.Create(newRole, cancellationToken);

                if (result.IsSuccess)
                    return Ok(newRole);
                else
                {
                    return Problem(
                          detail: result.Message,
                          title: "Kayıt ekleme");

                }
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }
        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDTO roleDTO, CancellationToken cancellationToken)
        {
            try
            {
                var updateRole = _mapper.Map<Genesis.Shared.Models.UserManagement.Roles>(roleDTO);
                

                var result = await _repository.Update(updateRole, cancellationToken);
                if (result.IsSuccess)
                    return Ok(updateRole);
                else
                {
                    return Problem(
                          detail: result.Message,
                          title: "Kayıt güncelleme");

                }
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
                
        }

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole([FromQuery] int id, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _repository.Delete(id, cancellationToken);
                if (result.IsSuccess)
                    return Ok(true);
                else
                {
                    return Problem(
                            detail: result.Message,
                            title: "Kayıt güncelleme");
                }

            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }
    }
}
