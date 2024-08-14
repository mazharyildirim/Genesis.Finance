using System.ComponentModel.DataAnnotations;

namespace Genesis.Shared.DTO
{
    public class UserDTO
    {
        public int userId { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }

        public int activeuserId { get; set; }
    }
}
