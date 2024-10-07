using Genesis.Shared.DTO;

namespace Genesis.Shared.UserRoleManagements
{
    public class UserResponse : ResponseBase
    {

        public List<UserListModel> List { get; set; } = new List<UserListModel>();

    }
}
