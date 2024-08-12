using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Genesis.Finance.Components;
using Genesis.Finance.Services;
var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IConsoleLogService, ConsoleLogService>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<System.Net.Http.HttpClient>();
builder.Services.AddTransient<UserService>();
builder.Services.AddScoped<StateService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
