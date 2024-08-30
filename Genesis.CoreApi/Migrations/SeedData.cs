using Genesis.CoreApi.Repository;
using Genesis.CoreApi.Shared.Cryptography;
using System;

namespace Genesis.CoreApi.Migrations
{
    public class SeedData
    {
        private readonly CoreDBContext _context;
        private readonly ISymmetricCryptographer _cryptographer;
        public SeedData(CoreDBContext context, ISymmetricCryptographer cryptographer)
        {
            _context = context;
            _cryptographer = cryptographer;
        }

        public void CreateData()
        {
            if (!_context.Users.Any())
            {
                string ps = _cryptographer.Encrypt("12345");

                var user = new Genesis.Shared.Models.UserManagement.Users()
                {
                    FirstName = "test",
                     MiddleName = "",
                    LastName = "test",
                    CreatedDate = DateTime.Now,
                    UserName = "admin",
                    IsActive =1,
                    Password = ps
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                var role = new Genesis.Shared.Models.UserManagement.Roles()
                {
                    RoleName = "admin",
                    IsActive =1,
                    CreatedDate = DateTime.Now
                };
                _context.Roles.Add(role);
                _context.SaveChanges();
                var userRoles = new Genesis.Shared.Models.UserManagement.UserRoles()
                {
                    RoleId = role.RoleId,
                    UserId = user.UserId
                };
                _context.UserRoles.Add(userRoles);
                _context.SaveChanges();
            }
        }
    }
}
