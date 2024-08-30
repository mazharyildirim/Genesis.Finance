using System;
using System.ComponentModel.DataAnnotations;
using Genesis.Shared.Models;

namespace Genesis.Shared.Models.UserManagement
{
    public class UserRoles: IManuallyIdentified
    {

        [Key]
        [Required]
        public int UserRolesId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

    }
}
