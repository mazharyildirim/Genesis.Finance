using Genesis.Shared.DTO;

namespace Genesis.Shared.Users
{
    public class UserResponse: ResponseBase
    {
       
        public List<UserListModel> List { get; set; } = new List<UserListModel>();
     
    }
}
