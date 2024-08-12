using Microsoft.EntityFrameworkCore;
using Genesis.Core.Models;
using Genesis.CoreApi.Shared;
using Microsoft.Extensions.Options;
using System.Data;

namespace Genesis.CoreApi.Repository
{
    public class CoreDBContext : DbContext
    {
       
        
        public CoreDBContext(DbContextOptions<CoreDBContext> options)  : base(options)
        { }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Users>().HasKey(c => c.UserId);
            modelBuilder.Entity<Roles>().ToTable("Roles");
            modelBuilder.Entity<Roles>().HasKey(c => c.RoleId);
            modelBuilder.Entity<UserRoles>().ToTable("UserRoles");
            modelBuilder.Entity<UserRoles>().HasKey(c => c.UserRolesId);

        }

       

        public virtual IDbConnection GetDbConnection()
        {
            return this.Database.GetDbConnection();
        }
    }
}
