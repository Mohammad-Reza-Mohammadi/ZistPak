using Data.Contracts;
using Entities.Useres;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utility.SwaggerConfig;

namespace Services.Services
{
    public class JwtSevice : IJwtSevice
    {
        private readonly AppSettings _appSettings;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository userRepository;
        private readonly UserManager<User> _userManager;

        public JwtSevice(IOptionsSnapshot<AppSettings> settings, SignInManager<User> signInManager, IUserRepository userRepository,UserManager<User> userManager)
        {
            _appSettings = settings.Value;
            _signInManager = signInManager;
            this.userRepository = userRepository;
            this._userManager = userManager;
        }
        public async Task<AccessToken> GenerateAsync(User user)
        {

            var secretKey = Encoding.UTF8.GetBytes(_appSettings.JwtSettings.Secretkey);// longer than 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(_appSettings.JwtSettings.Encryptkey);// longer than 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = await _getClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _appSettings.JwtSettings.Issuer,
                Audience = _appSettings.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_appSettings.JwtSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(_appSettings.JwtSettings.ExpirationMinutes),
                SigningCredentials = signingCredentials,// سازنده امضای توکن jwt
               // EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
            //var jwt = tokenHandler.WriteToken(securityToken);
            //await userRepository.UpdateAsync(user, cancellationToken);
            return new AccessToken(securityToken );
         }

        private async Task<IEnumerable<Claim>> _getClaimsAsync(User user)
        {
            var principal = await _signInManager.ClaimsFactory.CreateAsync(user);
            var claims = principal.Claims;


            return claims;

        }
    }
}
#region
//    var claims = new List<Claim>();

//    // Add custom claims
//    claims.Add(new Claim("custom-claim-type", "custom-claim-value"));

//    // Get the roles of the user and add them as claims
//    var roles = await userManager.GetRolesAsync(user);
//    foreach (var role in roles)
//    {
//        claims.Add(new Claim(ClaimTypes.Role, role));
//    }

//    // Get the claims of the user and add them to the list of claims
//    var userClaims = await userManager.GetClaimsAsync(user);
//    claims.AddRange(userClaims);

//    return claims;



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

#endregion
