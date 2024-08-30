
namespace Genesis.CoreApi.Repository
{
    public class AuthRepository: IAuthRepository
    {
        private CoreDBContext _context;
        public AuthRepository(CoreDBContext context)
        {
            _context = context;
        }
        public void UpdateUserRefreshTokens(string username, string oldRefleshToken, string refleshToken)
        {
            var usersData = _context.Users.FirstOrDefault(r => r.UserName == username && r.RefreshToken == oldRefleshToken && r.IsActive == 1 && r.IsDeleted == 0);
            if (usersData != null)
            {
                usersData.RefreshToken = refleshToken;
            }
            else
            {
                throw new Exception("Kullanıcı bilgisi bulunamadı.");
            }
            _context.Update(usersData);
            _context.SaveChanges();
        }

        public Genesis.Shared.Models.UserManagement.Users Find(string username, string password)
        {
            return  _context.Users.FirstOrDefault(r => r.UserName == username && r.Password == password);
        }

        public Genesis.Shared.Models.UserManagement.Users FindByRefleshToken(string username, string refleshToken)
        {
            return _context.Users.FirstOrDefault(r => r.UserName == username && r.RefreshToken == refleshToken && r.IsActive == 1 && r.IsDeleted == 0);
        }
    }
}
