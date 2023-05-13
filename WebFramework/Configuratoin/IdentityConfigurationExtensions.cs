using Data;
using Entities.Useres;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.SwaggerConfig;

namespace WebFramework.Configuratoin
{
    public static class IdentityConfigurationExtensions
    {
        public static void AddCustomIdentity(this IServiceCollection services, IdentitySettings settings)
        {
            services.AddIdentity<User, Role>(identityOptions =>
            {
                //Password Settings
                identityOptions.Password.RequireDigit = settings.PasswordRequireDigit;
                identityOptions.Password.RequireNonAlphanumeric = settings.PasswordRequireNonAlphanumic;
                identityOptions.Password.RequiredLength = settings.PasswordRequiredLength;
                identityOptions.Password.RequireUppercase = settings.PasswordRequireUppercase;
                identityOptions.Password.RequireLowercase = settings.PasswordRequireLowercase;

                //UserName Settings
                identityOptions.User.RequireUniqueEmail = settings.RequireUniqueEmail;

                ////Signin Settings
                //identityOptions.SignIn.RequireConfirmedPhoneNumber = false;
                //identityOptions.SignIn.RequireConfirmedEmail = false;

                //work when use coocki not jwt
                ////Lockout Settings
                //identityOptions.Lockout.MaxFailedAccessAttempts = 5; ////default =5
                //identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //identityOptions.Lockout.AllowedForNewUsers = false;
            }).AddEntityFrameworkStores<ZPakContext>()
            .AddDefaultTokenProviders();
        }
    }
}
