using Common.Utilities;
using Data.Contracts;
using Entities.Useres;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Utility.Api;
using Utility.Exceptions;
using Utility.SwaggerConfig;


namespace WebFramework.Configuratoin
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthorization(options =>
            {
                //UserPolicy
                options.AddPolicy("GetAllUsersPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();// برای همه کاربرانی که وارد سیستم شده‌اند، این Policy را اعمال کنید
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Admin") ||// برای افرادی که نقش Admin دارند، این Policy را اعمال کنید
                        context.User.HasClaim("GetAllUser", "true"));// برای افرادی که Claim با عنوان GetAllUser و مقدار "true" را دارند، این Policy را اعمال کنید
                        
                });
                options.AddPolicy("GetUserByIdPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();// برای همه کاربرانی که وارد سیستم شده‌اند، این Policy را اعمال کنید
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Admin") ||// برای افرادی که نقش Admin دارند، این Policy را اعمال کنید
                        context.User.HasClaim("GetUserById", "true"));// برای افرادی که Claim با عنوان GetUserById و مقدار "true" را دارند، این Policy را اعمال کنید

                });
                options.AddPolicy("ActiveUserAdminPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();// برای همه کاربرانی که وارد سیستم شده‌اند، این Policy را اعمال کنید
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Admin") ||// برای افرادی که نقش Admin دارند، این Policy را اعمال کنید
                        context.User.HasClaim("ActiveUserAdmin", "true"));// برای افرادی که Claim با عنوان GetUserById و مقدار "true" را دارند، این Policy را اعمال کنید

                });
                options.AddPolicy("ActiveUserMuniciaplityPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();// برای همه کاربرانی که وارد سیستم شده‌اند، این Policy را اعمال کنید
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Municipality") ||// برای افرادی که نقش Admin دارند، این Policy را اعمال کنید
                        context.User.HasClaim("ActiveUserMuniciaplity", "true"));// برای افرادی که Claim با عنوان ActiveUserMuniciaplity و مقدار "true" را دارند، این Policy را اعمال کنید

                });
                options.AddPolicy("ActiveUserCotractorPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();// برای همه کاربرانی که وارد سیستم شده‌اند، این Policy را اعمال کنید
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Contractor") ||// برای افرادی که نقش Admin دارند، این Policy را اعمال کنید
                        context.User.HasClaim("ActiveUserCotractor", "true"));// برای افرادی که Claim با عنوان ActiveUserCotractor و مقدار "true" را دارند، این Policy را اعمال کنید

                });
                //ItemPolicy
                options.AddPolicy("AddItemPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();// برای همه کاربرانی که وارد سیستم شده‌اند، این Policy را اعمال کنید
                    policy.RequireAssertion(context =>
                        context.User.HasClaim("AddItem", "true"));// برای افرادی که Claim با عنوان AddItem و مقدار "true" را دارند، این Policy را اعمال کنید

                });
                options.AddPolicy("UpdateItemPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();// برای همه کاربرانی که وارد سیستم شده‌اند، این Policy را اعمال کنید
                    policy.RequireAssertion(context =>
                        context.User.HasClaim("UpdateItem", "true"));// برای افرادی که Claim با عنوان UpdateItem و مقدار "true" را دارند، این Policy را اعمال کنید

                });
                options.AddPolicy("DeleteItemPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();// برای همه کاربرانی که وارد سیستم شده‌اند، این Policy را اعمال کنید
                    policy.RequireAssertion(context =>
                        context.User.HasClaim("DeleteItem", "true"));// برای افرادی که Claim با عنوان DeleteItem و مقدار "true" را دارند، این Policy را اعمال کنید

                });
                //CargoPolicy
                options.AddPolicy("AddCargoPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();// برای همه کاربرانی که وارد سیستم شده‌اند، این Policy را اعمال کنید
                    policy.RequireAssertion(context =>
                        context.User.HasClaim("AddCargo", "true"));// برای افرادی که Claim با عنوان AddCargo و مقدار "true" را دارند، این Policy را اعمال کنید

                });
                options.AddPolicy("UpdateCargoPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();// برای همه کاربرانی که وارد سیستم شده‌اند، این Policy را اعمال کنید
                    policy.RequireAssertion(context =>
                        context.User.HasClaim("UpdateCargo", "true"));// برای افرادی که Claim با عنوان UpdateCargo و مقدار "true" را دارند، این Policy را اعمال کنید

                });
                options.AddPolicy("DeleteCargoPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();// برای همه کاربرانی که وارد سیستم شده‌اند، این Policy را اعمال کنید
                    policy.RequireAssertion(context =>
                        context.User.HasClaim("DeleteCargo", "true"));// برای افرادی که Claim با عنوان DeleteCargo و مقدار "true" را دارند، این Policy را اعمال کنید

                });
                options.AddPolicy("GetAllCargoPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();// برای همه کاربرانی که وارد سیستم شده‌اند، این Policy را اعمال کنید
                    policy.RequireAssertion(context =>
                        context.User.HasClaim("GetAllCargo", "true"));// برای افرادی که Claim با عنوان DeleteCargo و مقدار "true" را دارند، این Policy را اعمال کنید

                });


            });
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
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("Authentication failed.", context.Exception);

                        if (context.Exception != null)
                            throw new AppException(ApiResultStatusCode.UnAuthorized, "Authentication failed.", HttpStatusCode.Unauthorized, context.Exception, null);

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        //var applicationSignInManager = context.HttpContext.RequestServices.GetRequiredService<IApplicationSignInManager>();
                        var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("This token has no claims.");

                        var securityStamp = claimsIdentity.FindFirst(new ClaimsIdentityOptions().SecurityStampClaimType);
                        if (securityStamp == null)
                            context.Fail("This token has no secuirty stamp");

                        //Find user and token from database and perform your custom validation
                        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                        var user = await userRepository.FindById(int.Parse(userId.ToString()));

                        //if (user.SecurityStamp != Guid.Parse(securityStamp))
                        //    context.Fail("Token secuirty stamp is not valid.");

                        var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                        if (validatedUser == null)
                            context.Fail("Token secuirty stamp is not valid.");

                        if (user.UserIsActive == false)
                            context.Fail("User is not active.");

                        await userRepository.UpdateLastLoginDateAsync(user, context.HttpContext.RequestAborted);
                    },
                    OnChallenge = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);

                        if (context.AuthenticateFailure != null)
                            throw new AppException(ApiResultStatusCode.UnAuthorized, "Authenticate failure.", HttpStatusCode.Unauthorized, context.AuthenticateFailure, null);
                        throw new AppException(ApiResultStatusCode.UnAuthorized, "You are unauthorized to access this resource.", HttpStatusCode.Unauthorized);

                        //return Task.CompletedTask;
                    }
                };
            });
        }
    }
}

#region
//OnAuthenticationFailed = context =>
//{

//    if (context.Exception != null)
//        throw new AppException(ApiResultStatusCode.UnAuthorized, "Authentication failed.", HttpStatusCode.Unauthorized, context.Exception, null);

//    return Task.CompletedTask;

//    //if (context.Exception != null)
//    //{
//    //    // Check if the exception is due to an invalid token
//    //    if (context.Exception.GetType() == typeof(SecurityTokenValidationException))
//    //    {
//    //        throw new AppException(ApiResultStatusCode.UnAuthorized, "Invalid token.", HttpStatusCode.Unauthorized, context.Exception, null);
//    //    }
//    //    else if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
//    //    {
//    //        throw new AppException(ApiResultStatusCode.UnAuthorized, "Token has expired.", HttpStatusCode.Unauthorized, context.Exception, null);
//    //    }
//    //    else
//    //    {
//    //        // Get the user id from the token
//    //        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
//    //        var handler = new JwtSecurityTokenHandler();
//    //        var decodedToken = handler.ReadJwtToken(token);
//    //        var userId = decodedToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;

//    //        // Parse the user id to integer
//    //        if (!int.TryParse(userId, out int userIdInt))
//    //        {
//    //            throw new AppException(ApiResultStatusCode.UnAuthorized, "Invalid user id.", HttpStatusCode.Unauthorized, context.Exception, null);
//    //        }

//    //        // Get the user by id
//    //        var user = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>().FindByIdAsync(userIdInt.ToString()).Result;

//    //        if (user == null)
//    //        {
//    //            throw new AppException(ApiResultStatusCode.UnAuthorized, "User not found.", HttpStatusCode.Unauthorized, context.Exception, null);
//    //        }
//    //        else
//    //        {
//    //            throw new AppException(ApiResultStatusCode.UnAuthorized, "Authentication failed.", HttpStatusCode.Unauthorized, context.Exception, null);
//    //        }
//    //    }
//    //}

//    //return Task.CompletedTask;
//},
#endregion

