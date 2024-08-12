using AutoMapper;
using Genesis.CoreApi.DTO;
using Genesis.CoreApi.Repository;
using Genesis.CoreApi.Shared;
using Genesis.CoreApi.Shared.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Genesis.CoreApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly ISymmetricCryptographer _cryptographer;
        private CoreDBContext _context;
        public UserController(IUserRepository repository, IMapper mapper, ISymmetricCryptographer cryptographer, CoreDBContext context) 
        {
            _repository = repository;
            _mapper = mapper;
            _cryptographer = cryptographer;
            _context = context;
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers(CancellationToken cancellationToken)
        {
            var users = _repository.GetAll().ToList();
            return Ok(_mapper.Map<List<UserDTO>>(users));
        }

      
        [HttpGet("GetUsers")]
        public async Task<ActionResult<ApiResult<UserListModel>>> GetUsers(
              int pageIndex = 0,
              int pageSize = 10,
              string? sortColumn = null,
              string? sortOrder = null,
              string? filterColumn = null,
              string? filterQuery = null)
        {
            return await ApiResult<UserListModel>.CreateAsync(
                    _context.Users.Where(r => r.IsDeleted == 0 && r.IsActive == 1).AsNoTracking()
                        .Select(c => new UserListModel()
                        {
                            userId = c.UserId,
                            firstname = c.FirstName,
                            lastname = c.LastName,
                            username = c.UserName,
                        }),
                    pageIndex,
                    pageSize,
                    sortColumn,
                    sortOrder,
                    filterColumn,
                    filterQuery);
        }

        [HttpGet("GetUserId")]
        public IActionResult GetUserId([FromQuery] int id, CancellationToken cancellationToken)
        {
            var user = _repository.GetId(id);
            var data = _mapper.Map<UserDTO>(user.Result.ResultData);
            return Ok(data);
        }

        
        [HttpPost("AddUser")]
        public async Task<Response<NoContent>> AddUser([FromBody] UserDTO userDTO, CancellationToken cancellationToken)
        {
            var newUser = _mapper.Map<Genesis.Core.Models.Users>(userDTO);
            newUser.CreatedUserId = userDTO.activeuserId;
            var result = await _repository.Create(newUser);

            if (result.IsSuccess)
                return Response<NoContent>.Success(204);
            else
            {
                return Response<NoContent>.Fail(!string.IsNullOrEmpty(result.Message) ? result.Message : string.Empty, 404);
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<Response<NoContent>> UpdateUser([FromBody] UserDTO userDTO, CancellationToken cancellationToken)
        {
            var updateUser = _mapper.Map<Genesis.Core.Models.Users>(userDTO);
            updateUser.CreatedUserId = userDTO.activeuserId;
            var result = await _repository.Update(updateUser);
            if (result.IsSuccess)
                return Response<NoContent>.Success(204);
            else
            {
                return Response<NoContent>.Fail(!string.IsNullOrEmpty(result.Message) ? result.Message : string.Empty, 404);
            }
        }

        [HttpPut("DeleteUser")]
        public async Task<Response<NoContent>> DeleteUser([FromQuery] int id, CancellationToken cancellationToken)
        {
            var result = await _repository.Delete(id);
            if (result.IsSuccess)
                return Response<NoContent>.Success(204);
            else
            {
                return Response<NoContent>.Fail(!string.IsNullOrEmpty(result.Message) ? result.Message : string.Empty, 404);
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<Response<NoContent>> ChangePassword([FromBody] UserDTO userDTO, CancellationToken cancellationToken)
        {
            var updateUser = _mapper.Map<Genesis.Core.Models.Users>(userDTO);
            string ps = _cryptographer.Encrypt(updateUser.Password);
            updateUser.Password = ps;
            updateUser.CreatedUserId = userDTO.activeuserId;
            var result = await _repository.ChangePassword(updateUser);
            if (result.IsSuccess)
                return Response<NoContent>.Success(204);
            else
            {
                return Response<NoContent>.Fail(!string.IsNullOrEmpty(result.Message) ? result.Message : string.Empty, 404);
            }
        }
    }

}
