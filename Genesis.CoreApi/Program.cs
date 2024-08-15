using AutoMapper;
using Genesis.CoreApi.Mapping;
using Genesis.CoreApi.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.MSSqlServer(connectionString:
                ctx.Configuration.GetConnectionString("MsSQLConnection"),
            restrictedToMinimumLevel: LogEventLevel.Information,
            sinkOptions: new MSSqlServerSinkOptions
            {
                TableName = "LogEvents",
                AutoCreateSqlTable = true
            }
            )
    .WriteTo.Console()
    );

builder.Services.Configure<Genesis.CoreApi.Models.Cryptographer>(builder.Configuration.GetSection("Cryptography"));

// Add services to the container.
builder.Services.AddDbContext<Genesis.CoreApi.Repository.CoreDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MsSQLConnection")));

builder.Services.AddTransient<Genesis.CoreApi.Shared.Cryptography.ISymmetricCryptographer, Genesis.CoreApi.Shared.Cryptography.AESCryptographer>();
builder.Services.AddTransient<Genesis.CoreApi.Repository.IUserRepository, Genesis.CoreApi.Repository.UserRepository>();
builder.Services.AddTransient<Genesis.CoreApi.Repository.IUserRolesRepository, Genesis.CoreApi.Repository.UserRolesRepository>();
builder.Services.AddTransient<Genesis.CoreApi.Repository.IRolesRepository, Genesis.CoreApi.Repository.RolesRepository>();
builder.Services.AddTransient<Genesis.CoreApi.Repository.IAuthRepository, Genesis.CoreApi.Repository.AuthRepository>();

// Auto Mapper Configurations
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new GeneralMapping());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
//add code auth.
builder.Services.AddAuthentication().AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = builder.Configuration["JWT:Issuer"],
         ValidAudience = builder.Configuration["JWT:Issuer"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
     };
 });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Genesis.CoreApi", Version = "v1" });
});



builder.Services.AddCors(options =>
    options.AddPolicy(name: "AngularPolicy",
        cfg => {
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
            cfg.WithOrigins(builder.Configuration["AllowedCORS"]);
        }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var context = service.GetService<Genesis.CoreApi.Repository.CoreDBContext>();
    var cryptographer = service.GetService<Genesis.CoreApi.Shared.Cryptography.ISymmetricCryptographer>();
    SeedData seedData = new SeedData(context, cryptographer);
    seedData.CreateData();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseCors("AngularPolicy");

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "api/{controller=Home}/{action=Index}/{id}");
});

app.Run();


