using Data;
using Data.Contracts;
using Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using Services.Services;
using Utility.SwaggerConfig;
using Utility.SwaggerConfig.Permissions;
using Utility.UserSecrets;
using WebFramework.Configuratoin;
using WebFramework.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

var AppSettingSections = builder.Configuration.GetSection(nameof(AppSettings));
builder.Services.Configure<Utility.SwaggerConfig.AppSettings>(AppSettingSections);
var appSettings = AppSettingSections.Get<AppSettings>();

builder.Services.AddCustomIdentity(appSettings.IdentitySettings);

//var AdminSecrert = builder.Configuration["MainAdmin:Name"];
builder.Configuration.GetSection("MainAdmin").Get<MainAdmin>();


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOurSwagger();

builder.Services.AddDbContext<ZPakContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ZPakServer")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICargoRepository , CargoRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IOrderRepository, OderRepository>();
builder.Services.AddScoped<IOrderDetailRepository , OrderDetailRepository>();
builder.Services.AddScoped<IJwtSevice, JwtSevice>();

//builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

builder.Services.AddJwtAuthentication(appSettings.JwtSettings);

var app = builder.Build();
app.UseCustomExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
}
else
{
    //app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();

app.UseSwaggerAndUI();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
