using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utility.SwaggerConfig
{
    public static class AddAuthenticationExtension
    {
        public static IServiceCollection AddOurAuthentication(this IServiceCollection Services,AppSettings appSettings)
        {
            #region Policy base
            // Authorization service
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("GetAllUser",
            //        policy => policy.RequireClaim("AccessAllUser", "True"));
            //});
            #endregion

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });
            return Services;
        }
    }
}
