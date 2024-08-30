using Genesis.CoreApi.Shared;

namespace Genesis.CoreApi.Repository
{
    public interface IUserRepository: IRepository<Genesis.Shared.Models.UserManagement.Users>
    {
        Task<NProcessResult<bool>> ChangePassword(Genesis.Shared.Models.UserManagement.Users entity);
        Task<NProcessResult<Genesis.Shared.Models.UserManagement.Users>> GetUsername(string username);
    }
}
