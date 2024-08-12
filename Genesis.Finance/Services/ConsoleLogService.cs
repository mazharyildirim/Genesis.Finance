﻿using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Genesis.Finance.Services
{
    public class ConsoleLogService : IConsoleLogService
    {
        private readonly IJSRuntime jsRuntime;

        public ConsoleLogService(IJSRuntime _jsRuntime)
        {
            jsRuntime = _jsRuntime;
        }

        public async Task<bool> LogAsync(string LogString)
        {
            return await jsRuntime.InvokeAsync<bool>("Realworld.consoleLog", LogString);
        }
    }
}
