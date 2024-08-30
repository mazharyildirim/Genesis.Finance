using Genesis.CoreApi.Shared;
using Microsoft.EntityFrameworkCore;

namespace Genesis.CoreApi.Repository
{
    public class UserRolesRepository: IUserRolesRepository
    {
        private CoreDBContext _context;
        public UserRolesRepository(CoreDBContext context) 
        {
            _context = context;
        }

        public List<string> GetRoles(int userId)
        {
            var lsRoles = (from r in _context.Roles
                        join us in _context.UserRoles on r.RoleId equals us.RoleId
                        select r.RoleName).ToList<string>();

            return lsRoles;
           
        }

        public async Task<NProcessResult<bool>> AddUserRole(int userId,int roleId)
        {
            NProcessResult<bool> result = new NProcessResult<bool>();
            _context.UserRoles.Add(new Genesis.Shared.Models.UserManagement.UserRoles()
            {
                 UserId =userId,
                 RoleId = roleId,
                  UserRolesId =-1
            });
            await _context.SaveChangesAsync();
            result.IsSuccess = true;

            return result;
        }

        public async Task<NProcessResult<bool>> DeleteByUser(int userId)
        {
            NProcessResult<bool> result = new NProcessResult<bool>();
            var userRoles = _context.UserRoles.Where(r=>r.UserId == userId);

            if (userRoles.Any())
            {
                _context.UserRoles.RemoveRange(userRoles);
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "not exists record.";
            }
            return result;

            return result;
        }
    }
}
