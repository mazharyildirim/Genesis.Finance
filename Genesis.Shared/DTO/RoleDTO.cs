using System.ComponentModel.DataAnnotations;

namespace Genesis.Shared.DTO
{
    public class RoleDTO
    {
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Rol adı giriniz.")]
        public string RoleName { get; set; }
        public int activeuserId { get; set; }

    }
}
