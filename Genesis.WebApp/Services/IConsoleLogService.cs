using System.Threading.Tasks;

namespace Genesis.WebApp.Services
{
    public interface IConsoleLogService
    {
        Task<bool> LogAsync(string LogString);
    }
}
