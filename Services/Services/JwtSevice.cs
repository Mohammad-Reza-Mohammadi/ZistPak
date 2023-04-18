using Data.Contracts;
using Entities.Useres;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utility.SwaggerConfig;
using Utility.SwaggerConfig.Permissions;

namespace Services.Services
{
    public class JwtSevice : IJwtSevice
    {
        private readonly AppSettings _appSettings;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository userRepository;

        public JwtSevice(IOptionsSnapshot<AppSettings> settings,SignInManager<User> signInManager,IUserRepository userRepository)
        {
            _appSettings = settings.Value;
            _signInManager = signInManager;
            this.userRepository = userRepository;
        }
        public async Task<string> GenerateAsync(User user,CancellationToken cancellationToken)
        {

            var secretKey = Encoding.UTF8.GetBytes(_appSettings.JwtSettings.Secretkey);// longer than 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            //var encryptionkey = Encoding.UTF8.GetBytes(_appSettings.JwtSettings.Encryptkey);// longer than 16 character
            //var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW,SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = await _getClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _appSettings.JwtSettings.Issuer,
                Audience = _appSettings.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_appSettings.JwtSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(_appSettings.JwtSettings.ExpirationMinutes),
                //EncryptingCredentials = encryptingCredentials,
                SigningCredentials = signingCredentials,// سازنده امضای توکن jwt
                Subject = new ClaimsIdentity(claims),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(descriptor);
            user.UserToken = tokenHandler.WriteToken(securityToken);
            await userRepository.UpdateAsync(user,cancellationToken);
            
           

            return user.UserToken;
        }

        private async  Task<IEnumerable<Claim>> _getClaimsAsync(User user)
        {
            if(user is User)
            {
            var result = await _signInManager.ClaimsFactory.CreateAsync(user);

            var list = new List<Claim>(result.Claims);
            return list;
            }
            return null;
            //add custom claims 
            //list.Add(new Claim(ClaimTypes.MobilePhone, "09192633377"));



            //var per = user.UserPermissions;
            //var list = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name,user.UserName),
            //    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            //};

            //foreach (var permisson in per)
            //{
            //    list.Add(new Claim(Permissions.Permission, permisson.Permission.ToString()));
            //    list.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            //};

            //var roles = new Role[] { new Role { Name = "Admin" } };

            //foreach (var role in roles)
            //    list.Add(new Claim(ClaimTypes.Role, role.Name));

            //return list;
        }
    }
}
