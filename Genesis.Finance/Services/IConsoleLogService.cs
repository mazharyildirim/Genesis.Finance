using System.Threading.Tasks;

namespace Genesis.Finance.Services
{
    public interface IConsoleLogService
    {
        Task<bool> LogAsync(string LogString);
    }
}
