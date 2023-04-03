using Data;
using Data.Contracts;
using Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Utility.SwaggerConfig;
using Utility.SwaggerConfig.Permissions;

var builder = WebApplication.CreateBuilder(args);

// گرفتن secret key از appstteing.json
var AppSettingSections = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<Utility.SwaggerConfig.AppSettings>(AppSettingSections);
var secret = AppSettingSections.Get<AppSettings>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOurAuthentication(secret);
builder.Services.AddOurSwagger();

builder.Services.AddDbContext<ZPakContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ZPakServer")));

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ICargoRepository , CargoRepository>();
builder.Services.AddTransient<IItemRepository, ItemRepository>();
builder.Services.AddTransient<IMunicipalityRepository, MunicipalityRepository>();
builder.Services.AddTransient<IBasketRepository, BasketRepository>();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
