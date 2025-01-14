﻿using Data;
using Data.Contracts;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Services;
using Utility.SwaggerConfig;
using WebFramework.Configuratoin;
using WebFramework.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

var AppSettingSections = builder.Configuration.GetSection(nameof(AppSettings));
builder.Services.Configure<Utility.SwaggerConfig.AppSettings>(AppSettingSections);
var appSettings = AppSettingSections.Get<AppSettings>();

builder.Services.AddCustomIdentity(appSettings.IdentitySettings);


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
