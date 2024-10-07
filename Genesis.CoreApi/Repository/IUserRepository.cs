using Genesis.CoreApi.Shared;

namespace Genesis.CoreApi.Repository
{
    public interface IUserRepository: IRepository<Genesis.Shared.Models.UserManagement.Users>
    {
        Task<NProcessResult<bool>> ChangePassword(int userId, string password, int updatedBy, CancellationToken cancellationToken);
        Task<NProcessResult<Genesis.Shared.Models.UserManagement.Users>> GetUsername(string username, CancellationToken cancellationToken);


    }
}
