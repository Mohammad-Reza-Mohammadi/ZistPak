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
using static Utility.SwaggerConfig.Permissions.Permissions;
using Municipality = Entities.Municipality.Municipality;
using User = Entities.Useres.User;
using Entities.User.UserProprety.EnumProperty;
using Utility.SwaggerConfig.Permissions;

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

            municipality.municipalityPermissions = new List<MunicipalityPermissions>
            {
                new MunicipalityPermissions()
                {
                    Permission = "Permissions.Municipality.AddSoperviserPermissionById",
                },
                new MunicipalityPermissions()
                {
                    Permission = "Permissions.Municipality.AddAllSoperviserPermission",  
                },
            };

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

            var muniId = municipality.Id;

            var ListPermissions = await DbContext.Set<MunicipalityPermissions>().Where(u => u.municiaplityId == muniId).ToListAsync();

            // authenticaiton successfo so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var claims = new ClaimsIdentity();
            foreach (var permisson in ListPermissions)
            {
                claims.AddClaims(new[]{
                new Claim(Permissions.Permission,permisson.Permission.ToString()),
                new Claim(ClaimTypes.NameIdentifier,municipality.Id.ToString())
                }) ;

            }
            #region Claim or Policy
            //باید به در قسمت اضافه کردن policy ها یک policy جدید اضافه کنیم
            //Claim Base (in Sevice.Authorization add section)
            //new Claim("AccessAlluser" , user.AccsessAllUser.ToString()),////user.AccsessAllUser==bool property in user table
            #endregion
     
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            municipality.token = tokenHandler.WriteToken(token);

            //The password will not be returned
            municipality.municipalityPermissions = null;
            municipality.Hashpassword = null;
            return municipality;

        }

        public async Task<bool> ChangePermissinByID(int id, CancellationToken cancellationToken)
        {

            User User = await DbContext.Set<User>().Where(u => u.Id == id).SingleOrDefaultAsync(cancellationToken);
            var UserRole = User.Role;
            if (UserRole != Role.Supervisor)
            {
                return false;
            }
            User.permissions = new List<UPermissions>
            {
                new UPermissions()
                {
                    userId = id,
                    Permission = "Permissions.Cargo.AddCargo"
                },
                new UPermissions()
                {
                    userId = id,
                    Permission = "Permissions.Cargo.UpdateCargo"
                },
                new UPermissions()
                {
                    userId = id,
                    Permission = "Permissions.Cargo.DeleteCargo"
                },

            };

            var p = User.permissions;
            await DbContext.Set<UPermissions>().AddRangeAsync(p,cancellationToken);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task AllSupervisorChangePermissin(CancellationToken cancellationToken)
        {

            List<User> Users = await DbContext.Set<User>().Where(u => u.Role == Role.Supervisor).ToListAsync();

            foreach(var user in Users)
            {
                List<UPermissions> AddPermission = new List<UPermissions>
                {
                    new UPermissions()
                    {
                        userId = user.Id,
                        Permission = "Permissions.Cargo.AddCargo",
                    },
                    new UPermissions()
                    {
                        userId = user.Id,
                        Permission = "Permissions.Cargo.UpdateCargo"
                    },
                    new UPermissions()
                    {
                        userId = user.Id,
                        Permission = "Permissions.Cargo.DeleteCargo"
                    },
                };

                await DbContext.Set<UPermissions>().AddRangeAsync(AddPermission, cancellationToken);
                await DbContext.SaveChangesAsync();
            }

        }



    }
}
