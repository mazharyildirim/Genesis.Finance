using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Genesis.WebApp.Services
{
    public class JwtService : IJwtService
    {
        private readonly IJSRuntime jsRuntime;

        public JwtService(IJSRuntime _jsRuntime)
        {
            jsRuntime = _jsRuntime;
        }

        public async Task<string> GetTokenAsync()
        {
            return await jsRuntime.InvokeAsync<string>("GenesisWeb.getToken");
        }

        public async Task<bool> SaveTokenAsync(string Token,string username)
        {
            return await jsRuntime.InvokeAsync<bool>("GenesisWeb.saveToken", Token,username);
        }

        public async Task<bool> DestroyTokenAsync()
        {
            return await jsRuntime.InvokeAsync<bool>("GenesisWeb.destroyToken");
        }

        public async Task<string> GetUserNameAsync()
        {
            return await jsRuntime.InvokeAsync<string>("GenesisWeb.getusername");
        }
    }
}
