using Data.Contracts;
using Entities.Municipality;
using Entities.Municipality.Enum;
using Entities.Useres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utility.SwaggerConfig;
using Utility.Utility;

namespace Data.Repositories
{
    public class MunicipalityRepository : Repository<Municipality>, IMunicipalityRepository
    {
        private readonly AppSettings _appSettings;

        public MunicipalityRepository(ZPakContext dbContext, IOptions<AppSettings> appSettings)
            : base(dbContext)
        {
            _appSettings = appSettings.Value;
        }

   

        public async Task AddAsync(Municipality municipality, string password, CancellationToken cancellationToken)
        {
            var passswordHash = SecurityHelper.GetSha256Hash(password);
            municipality.Hashpassword = passswordHash;
            await base.AddAsync(municipality, cancellationToken);
            return;
        }

        public async Task<Municipality> LoginAsync(string name, string password, CancellationToken cancellationToken)
        {
            var passswordHash = SecurityHelper.GetSha256Hash(password);
            var municipality = await Table.Where(u => u.Name == name && u.Hashpassword == passswordHash).SingleOrDefaultAsync();
            if (municipality == null)
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
                new Claim(ClaimTypes.NameIdentifier , municipality.Id.ToString()),     
                new Claim(ClaimTypes.Role , municipality.Region.ToString()),     


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
            municipality.token = tokenHandler.WriteToken(token);

            //The password will not be returned
            municipality.Hashpassword = null;
            return municipality;

        }

    }
}
