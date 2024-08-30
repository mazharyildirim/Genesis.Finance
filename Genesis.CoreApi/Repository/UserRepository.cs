
using Genesis.CoreApi.Shared;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Genesis.CoreApi.Repository
{
    public class UserRepository: IUserRepository
    {
        private CoreDBContext _context;

        public UserRepository(CoreDBContext context) 
        {
            _context = context;
        }

        public async Task<NProcessResult<Genesis.Shared.Models.UserManagement.Users>> Create(Genesis.Shared.Models.UserManagement.Users user)
        {
            NProcessResult< Genesis.Shared.Models.UserManagement.Users > result = new NProcessResult<Genesis.Shared.Models.UserManagement.Users>();
            var newUser = user;
            newUser.IsActive = 1;
            newUser.CreatedDate = DateTime.Now;
            newUser.IsDeleted = 0;
            newUser.RefreshToken = string.Empty;
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            result.IsSuccess = true;
            result.ResultData = newUser;
            
            return result;
        }

        public async Task<NProcessResult<bool>> Delete(int userId)
        {
            NProcessResult<bool> result = new NProcessResult<bool>();
            var user = await _context.Users.FindAsync(userId);

            if (user != null)
            {
                if (_context.UserRoles.Any(r => r.UserId == userId))
                {
                    result.IsSuccess = false;
                    result.Message = "use user record";
                    return result;
                }

                _context.Users.Remove(user);
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

        public List<Genesis.Shared.Models.UserManagement.Users> GetAll()
        {
            return _context.Users.Where(r => r.IsDeleted == 0 && r.IsActive == 1).ToList<Genesis.Shared.Models.UserManagement.Users>();
        }

        public async Task<NProcessResult<Genesis.Shared.Models.UserManagement.Users>> GetId(int id)
        {
            NProcessResult<Genesis.Shared.Models.UserManagement.Users> result = new NProcessResult<Genesis.Shared.Models.UserManagement.Users>();
            var user = await _context.Users.FindAsync(id);
            result.IsSuccess = true;
            result.ResultData = user;
            return result;
        }

        public async Task<NProcessResult<bool>> Update(Genesis.Shared.Models.UserManagement.Users user)
        {
            NProcessResult<bool> result = new NProcessResult<bool>();

            var existsUpdateUser = _context.Users.FirstOrDefault(r => r.UserId == user.UserId);
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

        public async Task<NProcessResult<bool>> ChangePassword(Genesis.Shared.Models.UserManagement.Users user)
        {
            NProcessResult<bool> result = new NProcessResult<bool>();

            var existsUpdateUser = _context.Users.FirstOrDefault(r => r.UserId == user.UserId);
            if (existsUpdateUser != null)
            {
                existsUpdateUser.UpdatedDate = DateTime.Now;
                existsUpdateUser.UpdatedUserId = user.UpdatedUserId;
             
                _context.Users.Update(existsUpdateUser);
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

        public async Task<NProcessResult<Genesis.Shared.Models.UserManagement.Users>> GetUsername(string username)
        {
            NProcessResult<Genesis.Shared.Models.UserManagement.Users> result = new NProcessResult<Genesis.Shared.Models.UserManagement.Users>();
            var user = await _context.Users.FirstOrDefaultAsync(r => r.UserName == username && r.IsActive == 1);
            result.IsSuccess = true;
            result.ResultData = user;
            return result;
        }
    }
}
