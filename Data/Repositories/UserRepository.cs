using Data.Contracts;
using Entities.Useres;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utility.SwaggerConfig;
using Utility.Utility;

namespace Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppSettings _appSettings;
        public UserRepository(ZPakContext dbContext, IOptions<AppSettings> appSettings)
            : base(dbContext)
        {
            _appSettings = appSettings.Value;
        }

        public  Task AddAsync(User user, string password, CancellationToken cancellationToken)
        {
            var passswordHash = SecurityHelper.GetSha256Hash(password);
            user.HashPassword = passswordHash;
            return base.AddAsync(user, cancellationToken);
        }

        public async Task<User> Login(string firstName, string password, CancellationToken cancellationToken)
        {
            var passswordHash = SecurityHelper.GetSha256Hash(password);
            var user = await Table.Where(u => u.FullName.FirstName == firstName && u.HashPassword == passswordHash).SingleOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            // authenticaiton successfo so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var claims = new ClaimsIdentity();
            claims.AddClaims(new[]
            {
                //Role Base
                new Claim(ClaimTypes.Role , user.Role.ToString()),

             #region Claim or Policy
                //باید به در قسمت اضافه کردن policy ها یک policy جدید اضافه کنیم
                //Claim Base (in Sevice.Authorization add section)
                //new Claim("AccessAlluser" , user.AccsessAllUser.ToString()),////user.AccsessAllUser==bool property in user table
             #endregion


            });
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            //The password will not be returned
            user.HashPassword = null;
            return user;

        }

        public Task UpdateUserAsync(User user,int userId,IFormFile userImageFile, CancellationToken cancellationToken)
        {
            string ImagPath = UserImageExtension.ImgeToString(userImageFile);
            user.Image = ImagPath;
            return base.UpdateAsync(user, cancellationToken);
            
        }
    }
}
