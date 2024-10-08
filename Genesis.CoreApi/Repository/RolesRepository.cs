﻿using System.Linq;
using AutoMapper;
using Genesis.CoreApi.Shared;
using System.Data;
using System.Linq.Dynamic.Core;
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

        public async Task<List<Genesis.Shared.Models.UserManagement.Roles>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Roles.Where(r => r.IsDeleted == 0 && r.IsActive == 1).ToDynamicListAsync<Genesis.Shared.Models.UserManagement.Roles>(cancellationToken);
        }

        public async Task<NProcessResult<Genesis.Shared.Models.UserManagement.Roles>> GetId(int roleId, CancellationToken cancellationToken)
        {
            NProcessResult<Genesis.Shared.Models.UserManagement.Roles> result = new NProcessResult<Genesis.Shared.Models.UserManagement.Roles>();
            var role = await _context.Roles.FindAsync(roleId,cancellationToken);
            result.IsSuccess = true;
            result.ResultData = role;
            return result;
        }

        public async Task<NProcessResult<Genesis.Shared.Models.UserManagement.Roles>> Create(Genesis.Shared.Models.UserManagement.Roles role, CancellationToken cancellationToken)
        {
            NProcessResult<Genesis.Shared.Models.UserManagement.Roles> result = new NProcessResult<Genesis.Shared.Models.UserManagement.Roles>();
            var newRole = role;
            newRole.IsActive = 1;
            newRole.CreatedDate = DateTime.Now;
            newRole.IsDeleted = 0;
            _context.Roles.Add(newRole);
            await _context.SaveChangesAsync(cancellationToken);
            result.IsSuccess = true;
            result.ResultData = newRole;
            return result;
        }

        public async Task<NProcessResult<bool>> Update(Genesis.Shared.Models.UserManagement.Roles role, CancellationToken cancellationToken)
        {
            NProcessResult<bool> result = new NProcessResult<bool>();
          
            var existsUpdateRole = _context.Roles.FirstOrDefault(r => r.RoleId == role.RoleId);
            if (existsUpdateRole != null)
            {
                existsUpdateRole.UpdatedDate = DateTime.Now;
                existsUpdateRole.UpdatedUserId = role.UpdatedUserId;
                existsUpdateRole.RoleName = role.RoleName;
                _context.Roles.Update(existsUpdateRole);
                await _context.SaveChangesAsync(cancellationToken);
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "not exists record.";
            }
            return result;
        }

        public async Task<NProcessResult<bool>> Delete(int roleId, CancellationToken cancellationToken)
        {
            NProcessResult<bool> result = new NProcessResult<bool>();
            var role = await _context.Roles.FindAsync(roleId, cancellationToken);

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
