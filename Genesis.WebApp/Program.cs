using Genesis.WebApp;
using Genesis.WebApp.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IConsoleLogService, ConsoleLogService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<RoleService>();


builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<System.Net.Http.HttpClient>();
builder.Services.AddScoped<Genesis.WebApp.Services.StateService>();
await builder.Build().RunAsync();
