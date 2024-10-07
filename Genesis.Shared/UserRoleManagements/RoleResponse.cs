using Genesis.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Shared.UserRoleManagements
{
    public class RoleResponse: ResponseBase
    {
        public List<RoleListModel> List { get; set; } = new List<RoleListModel>();
    }
}
