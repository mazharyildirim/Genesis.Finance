using Microsoft.EntityFrameworkCore;

using Genesis.CoreApi.Shared;
using Microsoft.Extensions.Options;
using System.Data;

namespace Genesis.CoreApi.Repository
{
    public class CoreDBContext : DbContext
    {
       
        
        public CoreDBContext(DbContextOptions<CoreDBContext> options)  : base(options)
        { }
        public DbSet<Genesis.Shared.Models.UserManagement.Users> Users { get; set; }
        public DbSet<Genesis.Shared.Models.UserManagement.Roles> Roles { get; set; }
        public DbSet<Genesis.Shared.Models.UserManagement.UserRoles> UserRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Genesis.Shared.Models.UserManagement.Users>().ToTable("Users");
            modelBuilder.Entity<Genesis.Shared.Models.UserManagement.Users>().HasKey(c => c.UserId);
            modelBuilder.Entity<Genesis.Shared.Models.UserManagement.Roles>().ToTable("Roles");
            modelBuilder.Entity<Genesis.Shared.Models.UserManagement.Roles>().HasKey(c => c.RoleId);
            modelBuilder.Entity<Genesis.Shared.Models.UserManagement.UserRoles>().ToTable("UserRoles");
            modelBuilder.Entity<Genesis.Shared.Models.UserManagement.UserRoles>().HasKey(c => c.UserRolesId);

        }

       

        public virtual IDbConnection GetDbConnection()
        {
            return this.Database.GetDbConnection();
        }
    }
}
