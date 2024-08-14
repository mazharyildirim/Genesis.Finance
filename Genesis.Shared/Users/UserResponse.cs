using Genesis.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Shared.Users
{
    public class UserResponse
    {
       
        public List<UserListModel> List { get; set; } = new List<UserListModel>();
    }
}
