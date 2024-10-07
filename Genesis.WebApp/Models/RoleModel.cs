using static System.Net.Mime.MediaTypeNames;

namespace Genesis.WebApp.Models
{
    public class RoleModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public RoleModel Clone()
        {
            return new RoleModel
            {
                RoleId = RoleId,
                RoleName = RoleName
            };
        }
    }
}
