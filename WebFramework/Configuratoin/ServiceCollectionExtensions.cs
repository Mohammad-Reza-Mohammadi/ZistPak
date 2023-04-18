using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Utility.SwaggerConfig;

namespace WebFramework.Configuratoin
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services,JwtSettings jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secretKey = Encoding.UTF8.GetBytes(jwtSettings.Secretkey);
                var encryptionkey = Encoding.UTF8.GetBytes(jwtSettings.Encryptkey);
                var validationParamerters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,// you can use your token after Zero minute of expire token(default: 5 min)
                    RequireSignedTokens = true,// token must have Signature

                    ValidateIssuerSigningKey = true, // signature of token must validate
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),//نحوه بررسی امضاء توکن 

                    RequireExpirationTime = true,// بررسی زمان انقضای توکن
                    ValidateLifetime = true,// بررسی لایف تایم توکن

                    ValidateAudience = true,// default : false
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuer = true,// default : false
                    ValidIssuer = jwtSettings.Issuer,

                    //TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
                };
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParamerters;

            }
                );
        }
    }
}
