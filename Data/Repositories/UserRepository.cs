using Data.Contracts;
using ECommerce.Utility;
using Entities.User.Owned;
using Entities.User.UserProprety.EnumProperty;
using Entities.Useres;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using presentation.Models;
using System.Security.Claims;
using Utility.Exceptions;
using Utility.SwaggerConfig;
using Utility.Utility;
using User = Entities.Useres.User;

namespace Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppSettings _appSettings;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserRepository(ZPakContext dbContext, IOptions<AppSettings> appSettings, UserManager<User> userManager, RoleManager<Role> roleManager)
            : base(dbContext)
        {
            _appSettings = appSettings.Value;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<Address> Getaddress(int userId)
        {
            return await DbContext.Set<Address>().Where(u => u.UserAddressOwnerId == userId).SingleOrDefaultAsync(); ;
        }

        public async Task<bool> GetUserByName(string userName)
        {
            User user = await DbContext.Set<User>().Where(u => u.UserName == userName).SingleOrDefaultAsync();
            if (user == null)
                return false;
            return true;
        }

        public async Task<bool> AddUserAsync(SignupUserDto signupUserDto, CancellationToken cancellationToken)
        {
            var exists = await TableNoTracking.AnyAsync(p => p.UserName == signupUserDto.UserName);

            if (exists)
            {
                throw new BadRequestException("نام کاربری تکراری است");
            }
            var user = new User
            {
                UserName = signupUserDto.UserName,
                Email = signupUserDto.Email,
                PhoneNumber = signupUserDto.phoneNumber,
                UserAge = signupUserDto.Age,
                UserGender = signupUserDto.Gender,
                CreateDate = DateTime.Today.ToShamsi(),
                SecurityStamp = Guid.NewGuid().ToString()

            };

            switch (signupUserDto.Role)
            {
                case UserRole.Admin:
                    {
                        var musnicipalityRole = new Role
                        {
                            Name = signupUserDto.Role.ToString(),
                            Description = "This is Admin Role",
                        };
                        var roleCreateResult = await _roleManager.CreateAsync(musnicipalityRole);
                        var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, musnicipalityRole.Name);

                        var claimList = new List<Claim>
                        {
                            new Claim("GetAllUser", "true"),
                            new Claim("GetUserById", "true"),
                        };
                        var AddUserCAndUserToDB = await _userManager.AddClaimsAsync(user, claimList);
                        break;
                    }
                case UserRole.Municipality:
                    {
                        user.UserIsActive = false;
                        var result = await _userManager.CreateAsync(user, signupUserDto.Password);
                        var musnicipalityRole = new Role
                        {
                            Name = signupUserDto.Role.ToString(),
                            Description = "This is Municipality Role",                            
                        };
                        var roleCreateResult = await _roleManager.CreateAsync(musnicipalityRole);
                        var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, musnicipalityRole.Name);

                        //var claimList = new List<Claim>
                        //{
                        //    new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),

                        //};
                        //var AddUserCAndUserToDB = await _userManager.AddClaimsAsync(user, claimList);
                        break;
                    }
                case UserRole.Supervisor:
                    {
                        user.UserIsActive = false;
                        var result = await _userManager.CreateAsync(user, signupUserDto.Password);

                        var musnicipalityRole = new Role
                        {
                            Name = signupUserDto.Role.ToString(),
                            Description = "This is Supervisor Role",
                        };
                        var roleCreateResult = await _roleManager.CreateAsync(musnicipalityRole);
                        var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, musnicipalityRole.Name);

                        //var claimList = new List<Claim>
                        //{
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //};
                        //var AddUserCAndUserToDB = await _userManager.AddClaimsAsync(user, claimList);
                        break;
                    }
                case UserRole.Contractor:
                    {
                        user.UserIsActive = false;
                        var result = await _userManager.CreateAsync(user, signupUserDto.Password);
                        var musnicipalityRole = new Role
                        {
                            Name = signupUserDto.Role.ToString(),
                            Description = "This is Contractor Role",
                        };
                        var roleCreateResult = await _roleManager.CreateAsync(musnicipalityRole);
                        var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, musnicipalityRole.Name);

                        //var claimList = new List<Claim>
                        //{
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //};
                        //var AddUserCAndUserToDB = await _userManager.AddClaimsAsync(user, claimList);
                        break;
                    }
                case UserRole.ExhibitorEmployee:
                    {
                        user.UserIsActive = false;
                        var result = await _userManager.CreateAsync(user, signupUserDto.Password);
                        var musnicipalityRole = new Role
                        {
                            Name = signupUserDto.Role.ToString(),
                            Description = "This is ExhibitorEmployee Role",
                        };
                        var roleCreateResult = await _roleManager.CreateAsync(musnicipalityRole);
                        var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, musnicipalityRole.Name);

                        //var claimList = new List<Claim>
                        //{
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //};
                        //var AddUserCAndUserToDB = await _userManager.AddClaimsAsync(user, claimList);
                        break;
                    }
                case UserRole.CarEmployee:
                    {
                        user.UserIsActive = false;
                        var result = await _userManager.CreateAsync(user, signupUserDto.Password);
                        var musnicipalityRole = new Role
                        {
                            Name = signupUserDto.Role.ToString(),
                            Description = "This is CarEmployee Role",
                        };
                        var roleCreateResult = await _roleManager.CreateAsync(musnicipalityRole);
                        var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, musnicipalityRole.Name);

                        //var claimList = new List<Claim>
                        //{
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //};
                        //var AddUserCAndUserToDB = await _userManager.AddClaimsAsync(user, claimList);
                        break;
                    }
                case UserRole.Customer:
                    {
                        var musnicipalityRole = new Role
                        {
                            Name = signupUserDto.Role.ToString(),
                            Description = "This is Customer Role",
                        };
                        var roleCreateResult = await _roleManager.CreateAsync(musnicipalityRole);
                        var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, musnicipalityRole.Name);

                        //var claimList = new List<Claim>
                        //{
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //    //new Claim("CanModifyCargo", "true"),
                        //};
                        //var AddUserCAndUserToDB = await _userManager.AddClaimsAsync(user, claimList);
                        break;
                    }
            }
            return true;
        }

        public async Task UpdateUserAsync(User user, int userId, IFormFile userImageFile, CancellationToken cancellationToken)
        {
            string ImagPath = UserImageExtension.ImgeToString(userImageFile);
            user.UserImage = ImagPath;
            await base.UpdateAsync(user, cancellationToken);
            return;

        }

        public async Task<bool> ChangePermissinByID(int id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return false;
            }

            if (await _userManager.IsInRoleAsync(user, "Supervisor"))
            {
                var claimList = new List<Claim>
                {
                    //ItemClaim
                    new Claim("AddItem", "true"),
                    new Claim("UpdateItem", "true"),
                    new Claim("DeleteItem", "true"),
                    //CargoClaim
                    new Claim("AddCargo", "true"),
                    new Claim("UpdateCargo", "true"),
                    new Claim("UpdateCargo", "true"),
                };
                var result = await _userManager.AddClaimsAsync(user, claimList);
                if (!result.Succeeded)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task AllSupervisorChangePermissin(CancellationToken cancellationToken)
        {

            var supervisors = await _userManager.GetUsersInRoleAsync("Supervisor");

            foreach (var supervisor in supervisors)
            {
                var claimList = new List<Claim>
                {
                    //ItemClaim
                    new Claim("AddItem", "true"),
                    new Claim("UpdateItem", "true"),
                    new Claim("DeleteItem", "true"),
                    //CargoClaim
                    new Claim("AddCargo", "true"),
                    new Claim("UpdateCargo", "true"),
                    new Claim("UpdateCargo", "true"),

                };
                var AddUserCAndUserToDB = await _userManager.AddClaimsAsync(supervisor, claimList);
            }
        }

        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTime.Now.ToShamsi();
            return UpdateAsync(user, cancellationToken);
        }
    }
}
