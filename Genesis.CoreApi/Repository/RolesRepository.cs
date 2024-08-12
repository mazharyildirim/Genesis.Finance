
using AutoMapper;
using EFCore.BulkExtensions;
using Genesis.Core.Models;
using Genesis.CoreApi.DTO;
using Genesis.CoreApi.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Linq;
namespace Genesis.CoreApi.Repository
{
    public class RolesRepository:  IRolesRepository
    {
        private CoreDBContext _context;
        private readonly IMapper _mapper;
        public RolesRepository(CoreDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Genesis.Core.Models.Roles> GetAll()
        {
            return _context.Roles.Where(r => r.IsDeleted == 0 && r.IsActive == 1).ToList<Genesis.Core.Models.Roles>();
        }

        public async Task<NProcessResult<Genesis.Core.Models.Roles>> GetId(int roleId)
        {
            NProcessResult<Genesis.Core.Models.Roles> result = new NProcessResult<Genesis.Core.Models.Roles>();
            var role = await _context.Roles.FindAsync(roleId);
            result.IsSuccess = true;
            result.ResultData = role;
            return result;
        }

        public async Task<NProcessResult<Genesis.Core.Models.Roles>> Create(Genesis.Core.Models.Roles role)
        {
            NProcessResult<Genesis.Core.Models.Roles> result = new NProcessResult<Genesis.Core.Models.Roles>();
            var newRole = role;
            newRole.IsActive = 1;
            newRole.CreatedDate = DateTime.Now;
            newRole.IsDeleted = 0;
            _context.Roles.Add(newRole);
            await _context.SaveChangesAsync();
            result.IsSuccess = true;
            result.ResultData = newRole;
            return result;
        }

        public async Task<NProcessResult<bool>> Update(Genesis.Core.Models.Roles role)
        {
            NProcessResult<bool> result = new NProcessResult<bool>();
          
            var existsUpdateRole = _context.Roles.FirstOrDefault(r => r.RoleId == role.RoleId);
            if (existsUpdateRole != null)
            {
                existsUpdateRole.UpdatedDate = DateTime.Now;
                existsUpdateRole.UpdatedUserId = role.UpdatedUserId;
                
                _context.Roles.Update(existsUpdateRole);
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "not exists record.";
            }
            return result;
        }

        public async Task<NProcessResult<bool>> Delete(int roleId)
        {
            NProcessResult<bool> result = new NProcessResult<bool>();
            var role = await _context.Roles.FindAsync(roleId);

            if (role != null)
            {
                if (_context.UserRoles.Any(r=>r.RoleId == roleId))
                {
                    result.IsSuccess = false;
                    result.Message = "use role record";
                    return result;
                }

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "not exists record.";
            }
            return result;
        }
    }
}
