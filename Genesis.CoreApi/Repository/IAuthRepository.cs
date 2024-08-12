namespace Genesis.CoreApi.Repository
{
    public interface IAuthRepository
    {
        Genesis.Core.Models.Users Find(string username, string password);
        Genesis.Core.Models.Users FindByRefleshToken(string username, string refleshToken);

        void UpdateUserRefreshTokens(string username, string oldRefleshToken, string refleshToken);
    }
}
