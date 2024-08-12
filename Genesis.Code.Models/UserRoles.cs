using System;
using System.ComponentModel.DataAnnotations;

namespace Genesis.Core.Models
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
