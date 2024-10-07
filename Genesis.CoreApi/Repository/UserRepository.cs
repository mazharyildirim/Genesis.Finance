
using Genesis.CoreApi.Shared;
using Genesis.Shared.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Genesis.CoreApi.Repository
{
    public class UserRepository: IUserRepository
    {
        private CoreDBContext _context;

        public UserRepository(CoreDBContext context) 
        {
            _context = context;
        }

        public async Task<NProcessResult<Genesis.Shared.Models.UserManagement.Users>> Create(Genesis.Shared.Models.UserManagement.Users user, CancellationToken cancellationToken)
        {
            NProcessResult< Genesis.Shared.Models.UserManagement.Users > result = new NProcessResult<Genesis.Shared.Models.UserManagement.Users>();
            var newUser = user;
            newUser.IsActive = 1;
            newUser.CreatedDate = DateTime.Now;
            newUser.IsDeleted = 0;
            newUser.RefreshToken = string.Empty;
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync(cancellationToken);
            result.IsSuccess = true;
            result.ResultData = newUser;
            
            return result;
        }

        public async Task<NProcessResult<bool>> Delete(int userId, CancellationToken cancellationToken)
        {
            NProcessResult<bool> result = new NProcessResult<bool>();
            var user = await _context.Users.FindAsync(userId, cancellationToken);

            if (user != null)
            {
                if (_context.UserRoles.Any(r => r.UserId == userId))
                {
                    result.IsSuccess = false;
                    result.Message = "use user record";
                    return result;
                }

                _context.Users.Remove(user);
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

        public async Task<NProcessResult<Genesis.Shared.Models.UserManagement.Users>> GetId(int id, CancellationToken cancellationToken)
        {
            NProcessResult<Genesis.Shared.Models.UserManagement.Users> result = new NProcessResult<Genesis.Shared.Models.UserManagement.Users>();
            var user = await _context.Users.FindAsync(id, cancellationToken);
            result.IsSuccess = true;
            result.ResultData = user;
            return result;
        }

        public async Task<NProcessResult<bool>> Update(Genesis.Shared.Models.UserManagement.Users user, CancellationToken cancellationToken)
        {
            NProcessResult<bool> result = new NProcessResult<bool>();

            var existsUpdateUser = await _context.Users.FirstOrDefaultAsync(r => r.UserId == user.UserId);
            if (existsUpdateUser != null)
            {
                existsUpdateUser.UpdatedDate = DateTime.Now;
                existsUpdateUser.UpdatedUserId = user.UpdatedUserId;
                existsUpdateUser.UserName = user.UserName;
                existsUpdateUser.FirstName = user.FirstName;
                existsUpdateUser.LastName = user.LastName;
                existsUpdateUser.MiddleName = user.MiddleName;
                existsUpdateUser.Email = user.Email;
                _context.Users.Update(existsUpdateUser);
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

        public async Task<NProcessResult<bool>> ChangePassword(int userId,string password,int updatedBy, CancellationToken cancellationToken)
        {
            NProcessResult<bool> result = new NProcessResult<bool>();

            var existsUpdateUser = await _context.Users.FirstOrDefaultAsync(r => r.UserId == userId,cancellationToken);
            if (existsUpdateUser != null)
            {
                existsUpdateUser.UpdatedDate = DateTime.Now;
                existsUpdateUser.UpdatedUserId = updatedBy;
                existsUpdateUser.Password = password;
                _context.Users.Update(existsUpdateUser);
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

        public async Task<NProcessResult<Genesis.Shared.Models.UserManagement.Users>> GetUsername(string username, CancellationToken cancellationToken)
        {
            NProcessResult<Genesis.Shared.Models.UserManagement.Users> result = new NProcessResult<Genesis.Shared.Models.UserManagement.Users>();
            var user = await _context.Users.FirstOrDefaultAsync(r => r.UserName == username && r.IsActive == 1,cancellationToken);
            result.IsSuccess = true;
            result.ResultData = user;
            return result;
        }

        public async Task<List<Users>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Users.Where(r => r.IsDeleted == 0 && r.IsActive == 1).ToDynamicListAsync<Genesis.Shared.Models.UserManagement.Users>(cancellationToken);
        }
    }
}
