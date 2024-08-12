using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Genesis.Finance.Services
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
            return await jsRuntime.InvokeAsync<string>("GenesisFinance.getToken");
        }

        public async Task<bool> SaveTokenAsync(string Token)
        {
            return await jsRuntime.InvokeAsync<bool>("GenesisFinance.saveToken", Token);
        }

        public async Task<bool> DestroyTokenAsync()
        {
            return await jsRuntime.InvokeAsync<bool>("GenesisFinance.destroyToken");
        }
    }
}
