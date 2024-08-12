using System;
using System.ComponentModel.DataAnnotations;
namespace Genesis.Core.Models
{
    public class Roles: Audit, IManuallyIdentified
    {

        [Key]
        [Required]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
       
    }

    
}
