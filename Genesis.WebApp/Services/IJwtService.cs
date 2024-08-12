using System.Threading.Tasks;

namespace Genesis.WebApp.Services
{
    public interface IJwtService
    {
        Task<string> GetTokenAsync();
        Task<string> GetUserNameAsync();
        Task<bool> SaveTokenAsync(string Token,string username);
        Task<bool> DestroyTokenAsync();
    }
}
