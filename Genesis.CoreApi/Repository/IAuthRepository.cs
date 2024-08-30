namespace Genesis.CoreApi.Repository
{
    public interface IAuthRepository
    {
        Genesis.Shared.Models.UserManagement.Users Find(string username, string password);
        Genesis.Shared.Models.UserManagement.Users FindByRefleshToken(string username, string refleshToken);

        void UpdateUserRefreshTokens(string username, string oldRefleshToken, string refleshToken);
    }
}
