﻿using AutoMapper;
using Genesis.CoreApi.Repository;
using Genesis.CoreApi.Shared;
using Genesis.CoreApi.Shared.Cryptography;
using Genesis.Shared;
using Genesis.Shared.DTO;
using Genesis.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Genesis.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly ISymmetricCryptographer _cryptographer;
        private Genesis.CoreApi.Repository.IUserRolesRepository _userRolesRepository;
        private CoreDBContext _context;
        public UserController(IUserRepository repository, IMapper mapper, ISymmetricCryptographer cryptographer, Genesis.CoreApi.Repository.IUserRolesRepository userRolesRepository, CoreDBContext context) 
        {
            _repository = repository;
            _mapper = mapper;
            _cryptographer = cryptographer;
            _userRolesRepository = userRolesRepository;
            _context = context;
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers(CancellationToken cancellationToken)
        {
            var users = _repository.GetAll().ToList();
            return Ok(_mapper.Map<List<UserDTO>>(users));
        }

      
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers(
              int pageIndex = 0,
              int pageSize = 10,
              string? sortColumn = null,
              string? sortOrder = null,
              string? filterColumn = null,
              string? filterQuery = null)
        {
         
           var data  = await ApiResult<UserListModel>.CreateAsync(
                    _context.Users.Where(r => r.IsDeleted == 0 && r.IsActive == 1).AsNoTracking()
                        .Select(c => new UserListModel()
                        {
                            userId = c.UserId,
                            firstname = c.FirstName,
                            lastname = c.LastName,
                            username = c.UserName,
                            email = c.Email
                        }),
                    pageIndex,
                    pageSize,
                    sortColumn,
                    sortOrder,
                    filterColumn,
                    filterQuery);
            Genesis.Shared.Users.UserResponse response = new UserResponse();
            response.List = data.Data;
            response.PageIndex = data.PageIndex;
            response.PageSize = data.PageSize;
            response.TotalCount = data.TotalCount;
            response.TotalPages = data.TotalPages;
            return Ok(response);
        }

        [HttpGet("GetUserId")]
        public IActionResult GetUserId([FromQuery] int id, CancellationToken cancellationToken)
        {
            var user = _repository.GetId(id);
            var data = _mapper.Map<UserDTO>(user.Result.ResultData);
            return Ok(data);
        }


        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] Genesis.Shared.DTO.UserDTO userDTO, CancellationToken cancellationToken)
        {
            try
            {
                var newUser = _mapper.Map<Genesis.Shared.Models.UserManagement.Users>(userDTO);
                newUser.CreatedUserId = userDTO.activeuserId;
                newUser.MiddleName = !string.IsNullOrEmpty(newUser.MiddleName) ? newUser.MiddleName : string.Empty;
                newUser.Password = !string.IsNullOrEmpty(newUser.Password) ? newUser.Password : string.Empty;
                var result = await _repository.Create(newUser);

                if (result.IsSuccess)
                    return Ok(newUser);
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

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] Genesis.Shared.DTO.UserDTO userDTO , CancellationToken cancellationToken)
        {
            try
            {
                var updateUser = _mapper.Map<Genesis.Shared.Models.UserManagement.Users>(userDTO);
                updateUser.CreatedUserId = userDTO.activeuserId;
           
                var result = await _repository.Update(updateUser);
                if (result.IsSuccess)
                    return Ok(updateUser);
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

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] int id, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _repository.Delete(id);
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

        [HttpPut("ChangePassword")]
        public async Task<Response<NoContent>> ChangePassword([FromBody] UserDTO userDTO, CancellationToken cancellationToken)
        {
            var updateUser = _mapper.Map<Genesis.Shared.Models.UserManagement.Users>(userDTO);
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
