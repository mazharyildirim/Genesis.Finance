using System.ComponentModel.DataAnnotations;

namespace Genesis.Shared.DTO
{
    public class UserDTO
    {
        public int userId { get; set; }

        [Required(ErrorMessage = "Ad giriniz.")]
        public string firstname { get; set; }

        [Required(ErrorMessage = "Soyad giriniz.")]
        public string lastname { get; set; }
        
        public string middlename { get; set; }
        
        [Required(ErrorMessage = "Kullanıcı adı giriniz.")]
        public string username { get; set; }

        public string password { get; set; }

        [Required(ErrorMessage = "Elektronik posta giriniz.")]
        public string email { get; set; }

        public int activeuserId { get; set; }
    }
}
