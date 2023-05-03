using Data.Contracts;
using ECommerce.Utility;
using Entities.Orders;
using Entities.User.Owned;
using Entities.User.UserProprety.EnumProperty;
using Entities.Useres;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using presentation.Models;
using System.Security.Claims;
using Utility.Exceptions;
using Utility.SwaggerConfig;
using Utility.Utility;
using static Utility.SwaggerConfig.Permissions.Permissions;
using Order = Entities.Orders.Order;
using User = Entities.Useres.User;

namespace Data.Repositories
{
    public class ClaimEqualityComparer : IEqualityComparer<Claim>
    {
        public static ClaimEqualityComparer Default { get; } = new ClaimEqualityComparer();

        public bool Equals(Claim x, Claim y)
        {
            return x.Type == y.Type;
        }

        public int GetHashCode(Claim obj)
        {
            return obj.Type.GetHashCode();
        }
    }
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppSettings _appSettings;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        //this is username and password of main admin
        private readonly string _adminName = "admin";
        private readonly string _adminPass = "1234567";

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
            var parentUserExists = await base.GetByIdAsync(cancellationToken, signupUserDto.ParetnUsereId);


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

            if (signupUserDto.UserName.Equals(_adminName) && signupUserDto.Password.Equals(_adminPass))
            {

                var result = await _userManager.CreateAsync(user, signupUserDto.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                // add user to database 
                var addUserResult = await _userManager.UpdateAsync(user);
                if (!addUserResult.Succeeded)
                {
                    throw new Exception(addUserResult.Errors.First().Description);
                }

                var AdminRole = new Role
                {
                    Name = signupUserDto.Role.ToString(),
                    Description = "This is Admin Role",
                };
                var roleCreateResult = await _roleManager.CreateAsync(AdminRole);
                var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, AdminRole.Name);

                var claimList = new List<Claim>
                        {
                            new Claim("GetAllUser", "true"),
                            new Claim("GetUserById", "true"),
                            new Claim("ActiveUserAdmin", "true"),
                        };
                var AddUserCAndUserToDB = await _userManager.AddClaimsAsync(user, claimList);
            }
            else
            {
                switch (signupUserDto.Role)
                {
                    case UserRole.Admin:
                        {
                            user.UserIsActive = false;
                            var result = await _userManager.CreateAsync(user, signupUserDto.Password);
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                            // add user to database 
                            var addUserResult = await _userManager.UpdateAsync(user);
                            if (!addUserResult.Succeeded)
                            {
                                throw new Exception(addUserResult.Errors.First().Description);
                            }

                            var AdminRole = new Role
                            {
                                Name = signupUserDto.Role.ToString(),
                                Description = "This is Admin Role",
                            };
                            var roleCreateResult = await _roleManager.CreateAsync(AdminRole);
                            var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, AdminRole.Name);

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
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                            // add user to database 
                            var addUserResult = await _userManager.UpdateAsync(user);
                            if (!addUserResult.Succeeded)
                            {
                                throw new Exception(addUserResult.Errors.First().Description);
                            }

                            var musnicipalityRole = new Role
                            {
                                Name = signupUserDto.Role.ToString(),
                                Description = "This is Municipality Role",
                            };
                            var roleCreateResult = await _roleManager.CreateAsync(musnicipalityRole);
                            var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, musnicipalityRole.Name);

                            var claimList = new List<Claim>
                            {
                                new Claim("ActiveUserMuniciaplity", "true"),
                                //new Claim("CanModifyCargo", "true"),

                            };
                            var AddUserCAndUserToDB = await _userManager.AddClaimsAsync(user, claimList);
                            break;
                        }
                    case UserRole.Supervisor:
                        {
                            if (!await _userManager.IsInRoleAsync(parentUserExists, "Municipality"))
                            {
                                throw new BadRequestException("شما نمیتوانید به این فرد یا ارگان مرتبط شوید");
                            }

                            user.UserIsActive = false;
                            var result = await _userManager.CreateAsync(user, signupUserDto.Password);
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                            // add user to database 
                            var addUserResult = await _userManager.UpdateAsync(user);
                            if (!addUserResult.Succeeded)
                            {
                                throw new Exception(addUserResult.Errors.First().Description);
                            }

                            var supervisorRole = new Role
                            {
                                Name = signupUserDto.Role.ToString(),
                                Description = "This is Supervisor Role",
                            };
                            var roleCreateResult = await _roleManager.CreateAsync(supervisorRole);
                            var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, supervisorRole.Name);

                            var claimList = new List<Claim>
                            {
                                 new Claim("GetAllCargo", "true"),
                                //new Claim("CanModifyCargo", "true"),
                                //new Claim("CanModifyCargo", "true"),
                                //new Claim("CanModifyCargo", "true"),
                                //new Claim("CanModifyCargo", "true"),
                                //new Claim("CanModifyCargo", "true"),
                            };
                            var AddUserCAndUserToDB = await _userManager.AddClaimsAsync(user, claimList);
                            break;
                        }
                    case UserRole.Contractor:
                        {
                            if (!await _userManager.IsInRoleAsync(parentUserExists, "Municipality"))
                            {
                                throw new BadRequestException("شما نمیتوانید به این فرد یا ارگان مرتبط شوید");
                            }
                            user.UserIsActive = false;
                            var result = await _userManager.CreateAsync(user, signupUserDto.Password);
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                            // add user to database 
                            var addUserResult = await _userManager.UpdateAsync(user);
                            if (!addUserResult.Succeeded)
                            {
                                throw new Exception(addUserResult.Errors.First().Description);
                            }

                            var contractorRole = new Role
                            {
                                Name = signupUserDto.Role.ToString(),
                                Description = "This is Contractor Role",
                            };
                            var roleCreateResult = await _roleManager.CreateAsync(contractorRole);
                            var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, contractorRole.Name);

                            var claimList = new List<Claim>
                            {
                                new Claim("ActiveUserCotractor", "true"),
                                //new Claim("CanModifyCargo", "true"),
                                //new Claim("CanModifyCargo", "true"),
                                //new Claim("CanModifyCargo", "true"),
                                //new Claim("CanModifyCargo", "true"),
                                //new Claim("CanModifyCargo", "true"),
                            };
                            var AddUserCAndUserToDB = await _userManager.AddClaimsAsync(user, claimList);
                            break;
                        }
                    case UserRole.ExhibitorEmployee:
                        {
                            if (!await _userManager.IsInRoleAsync(parentUserExists, "Contractor"))
                            {
                                throw new BadRequestException("شما نمیتوانید به این فرد یا ارگان مرتبط شوید");
                            }
                            user.UserIsActive = false;
                            var result = await _userManager.CreateAsync(user, signupUserDto.Password);
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                            // add user to database 
                            var addUserResult = await _userManager.UpdateAsync(user);
                            if (!addUserResult.Succeeded)
                            {
                                throw new Exception(addUserResult.Errors.First().Description);
                            }

                            var exhibitorEmployeeRole = new Role
                            {
                                Name = signupUserDto.Role.ToString(),
                                Description = "This is ExhibitorEmployee Role",
                            };
                            var roleCreateResult = await _roleManager.CreateAsync(exhibitorEmployeeRole);
                            var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, exhibitorEmployeeRole.Name);

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
                            if (!await _userManager.IsInRoleAsync(parentUserExists, "Contractor"))
                            {
                                throw new BadRequestException("شما نمیتوانید به این فرد یا ارگان مرتبط شوید");
                            }

                            user.UserIsActive = false;
                            var result = await _userManager.CreateAsync(user, signupUserDto.Password);
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                            // add user to database 
                            var addUserResult = await _userManager.UpdateAsync(user);
                            if (!addUserResult.Succeeded)
                            {
                                throw new Exception(addUserResult.Errors.First().Description);
                            }
                            var carEmployeeRole = new Role
                            {
                                Name = signupUserDto.Role.ToString(),
                                Description = "This is CarEmployee Role",
                            };
                            var roleCreateResult = await _roleManager.CreateAsync(carEmployeeRole);
                            var AddRoleAndUserToDB = await _userManager.AddToRoleAsync(user, carEmployeeRole.Name);

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
                            var result = await _userManager.CreateAsync(user, signupUserDto.Password);
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                            // add user to database 
                            var addUserResult = await _userManager.UpdateAsync(user);
                            if (!addUserResult.Succeeded)
                            {
                                throw new Exception(addUserResult.Errors.First().Description);
                            }
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
            var user = await base.GetByIdAsync(cancellationToken, id);
            if (user == null)
            {
                return false;
            }

            var existingClaims = await _userManager.GetClaimsAsync(user);


            var newClaims = new List<Claim>();

            // اضافه کردن کلیم‌هایی که کاربر هنوز ندارد
            if (!existingClaims.Any(c => c.Type == "AddItem"))
                newClaims.Add(new Claim("AddItem", "true"));
            if (!existingClaims.Any(c => c.Type == "UpdateItem"))
                newClaims.Add(new Claim("UpdateItem", "true"));
            if (!existingClaims.Any(c => c.Type == "DeleteItem"))
                newClaims.Add(new Claim("DeleteItem", "true"));
            if (!existingClaims.Any(c => c.Type == "AddCargo"))
                newClaims.Add(new Claim("AddCargo", "true"));
            if (!existingClaims.Any(c => c.Type == "UpdateCargo"))
                newClaims.Add(new Claim("UpdateCargo", "true"));
            if (!existingClaims.Any(c => c.Type == "DeleteCargo"))
                newClaims.Add(new Claim("DeleteCargo", "true"));

            // حذف کلیم‌های تکراری
            var distinctNewClaims = newClaims
                .Except(existingClaims, ClaimEqualityComparer.Default)
                .Distinct(ClaimEqualityComparer.Default);

            var claimsToRemove = existingClaims.Intersect(newClaims, ClaimEqualityComparer.Default);
            if (claimsToRemove.Any())
            {
                var result = await _userManager.RemoveClaimsAsync(user, claimsToRemove);
                if (!result.Succeeded)
                {
                    return false;
                }
            }

            if (distinctNewClaims.Any())
            {
                var result = await _userManager.AddClaimsAsync(user, distinctNewClaims);
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

                var existingClaims = await _userManager.GetClaimsAsync(supervisor);


                var newClaims = new List<Claim>();

                // اضافه کردن کلیم‌هایی که کاربر هنوز ندارد
                if (!existingClaims.Any(c => c.Type == "AddItem"))
                    newClaims.Add(new Claim("AddItem", "true"));
                if (!existingClaims.Any(c => c.Type == "UpdateItem"))
                    newClaims.Add(new Claim("UpdateItem", "true"));
                if (!existingClaims.Any(c => c.Type == "DeleteItem"))
                    newClaims.Add(new Claim("DeleteItem", "true"));
                if (!existingClaims.Any(c => c.Type == "AddCargo"))
                    newClaims.Add(new Claim("AddCargo", "true"));
                if (!existingClaims.Any(c => c.Type == "UpdateCargo"))
                    newClaims.Add(new Claim("UpdateCargo", "true"));
                if (!existingClaims.Any(c => c.Type == "DeleteCargo"))
                    newClaims.Add(new Claim("DeleteCargo", "true"));

                // حذف کلیم‌های تکراری
                var distinctNewClaims = newClaims
                    .Except(existingClaims, ClaimEqualityComparer.Default)
                    .Distinct(ClaimEqualityComparer.Default);

                var claimsToRemove = existingClaims.Intersect(newClaims, ClaimEqualityComparer.Default);
                if (claimsToRemove.Any())
                {
                    var result = await _userManager.RemoveClaimsAsync(supervisor, claimsToRemove);
                    if (!result.Succeeded)
                    {
                        throw new LogicException("مشکلی رخ داده است");
                    }
                }

                if (distinctNewClaims.Any())
                {
                    var result = await _userManager.AddClaimsAsync(supervisor, distinctNewClaims);
                    if (!result.Succeeded)
                    {
                        throw new LogicException("مشکلی رخ داده است");
                    }
                }
            }
        }

        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTime.Now.ToShamsi();
            return UpdateAsync(user, cancellationToken);
        }

        public async Task FinalizeThePurchase(int userId,CancellationToken cancellationToken)
        {
            // check the open order for CurrentUserId
            var order = DbContext.Set<Order>().Where(o => o.UserId == userId && !o.IsFinaly).SingleOrDefault();
            if (order.Equals(null))
                throw new LogicException("سبد خرید شما خالی است");
            var user = await base.GetByIdAsync(cancellationToken, userId);
            user.UserStar += order.OrderStar;
            order.IsFinaly=true;
            DbContext.UpdateRange(order, user);
            await DbContext.SaveChangesAsync();
            return;
            
        }
    }
}
