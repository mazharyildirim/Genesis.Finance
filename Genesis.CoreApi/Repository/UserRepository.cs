using Genesis.Core.Models;
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

        public async Task<NProcessResult<Users>> Create(Users user)
        {
            NProcessResult<Genesis.Core.Models.Users> result = new NProcessResult<Genesis.Core.Models.Users>();
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

        public List<Users> GetAll()
        {
            return _context.Users.Where(r => r.IsDeleted == 0 && r.IsActive == 1).ToList<Users>();
        }

        public async Task<NProcessResult<Users>> GetId(int id)
        {
            NProcessResult<Genesis.Core.Models.Users> result = new NProcessResult<Genesis.Core.Models.Users>();
            var user = await _context.Users.FindAsync(id);
            result.IsSuccess = true;
            result.ResultData = user;
            return result;
        }

        public async Task<NProcessResult<bool>> Update(Users user)
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

        public async Task<NProcessResult<bool>> ChangePassword(Users user)
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
    }
}
