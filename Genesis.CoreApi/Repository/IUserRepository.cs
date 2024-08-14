using Genesis.CoreApi.Shared;

namespace Genesis.CoreApi.Repository
{
    public interface IUserRepository: IRepository<Genesis.Core.Models.Users>
    {
        Task<NProcessResult<bool>> ChangePassword(Genesis.Core.Models.Users entity);
        Task<NProcessResult<Genesis.Core.Models.Users>> GetUsername(string username);
    }
}
