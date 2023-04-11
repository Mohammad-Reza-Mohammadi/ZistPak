using Data;
using Data.Contracts;
using Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Utility.SwaggerConfig;
using Utility.SwaggerConfig.Permissions;
using WebFramework.MiddleWares;

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

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICargoRepository , CargoRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IOrderRepository, OderRepository>();
builder.Services.AddScoped<IOrderDetailRepository , OrderDetailRepository>();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();


var app = builder.Build();
app.UseCustomExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    //app.UseExceptionHandler("/Error");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
