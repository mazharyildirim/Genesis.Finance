using System.ComponentModel.DataAnnotations;
namespace Genesis.Shared.Models.UserManagement
{
  
    public class Users : Audit, IManuallyIdentified
    {
        [Key]
        [Required]
        public int UserId { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(100)]
        public string RefreshToken { get; set; }



        public string GetName()
        {
            return FirstName + " " + MiddleName + " " + LastName;
        }
    }
}
