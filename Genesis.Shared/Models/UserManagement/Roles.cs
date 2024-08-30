using System;
using System.ComponentModel.DataAnnotations;
using Genesis.Shared.Models;
namespace Genesis.Shared.Models.UserManagement
{
    public class Roles: Audit, IManuallyIdentified
    {

        [Key]
        [Required]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
       
    }

    
}
