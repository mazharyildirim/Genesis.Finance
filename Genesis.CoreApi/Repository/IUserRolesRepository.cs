

using Genesis.CoreApi.Shared;

namespace Genesis.CoreApi.Repository
{
    public interface IUserRolesRepository
    {
        List<string> GetRoles(int userId);
        Task<NProcessResult<bool>> AddUserRole(int userId, int roleId);

        Task<NProcessResult<bool>> DeleteByUser(int userId);
    }
}
