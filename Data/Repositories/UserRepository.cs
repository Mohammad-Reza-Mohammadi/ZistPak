using Data.Contracts;
using ECommerce.Utility;
using Entities.User.Owned;
using Entities.User.UserProprety.EnumProperty;
using Entities.Useres;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utility.SwaggerConfig;
using Utility.SwaggerConfig.Permissions;
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

        public async Task AddUserAsync(User user, string password, CancellationToken cancellationToken)
        {
            var passswordHash = SecurityHelper.GetSha256Hash(password);
            user.UserPasswordHash = passswordHash;

            //به طور پیش فرض افراد فقط میتوانند به کنترلر هایی دسترسی داشته باشند که فقط نیاز به لاگین دارد
            // give permission
            switch (user.UserRole)
            {
                case UserRole.Municipality:
                    {
                        User user1 = new User()
                        {
                            CreateDate = DateTime.Today.ToShamsi(),
                            UserPasswordHash = user.UserPasswordHash,
                            UserName = user.UserName,
                            UserAge = user.UserAge,
                            UserGender = user.UserGender,
                            UserRole = user.UserRole,
                            //به طور پیش فرض ناظر نمیتواند محموله ای را تغییر دهد باید شهرداری مجوز را بدهد
                            #region add permissons
                            UserPermissions = new List<UPermissions>
                            {
                                new UPermissions()
                                {
                                    Permission = "Permissions.User.AddAllSoperviserPermission",
                                },
                                new UPermissions()
                                {
                                    Permission = "Permissions.User.AddSoperviserPermissionById"
                                }
                            }
                            #endregion
                        };

                        await base.AddAsync(user1, cancellationToken);
                        break;
                    }
                case UserRole.Supervisor:
                    {
                        User user1 = new User()
                        {
                            CreateDate = DateTime.Today.ToShamsi(),
                            UserPasswordHash = user.UserPasswordHash,
                            UserName = user.UserName,
                            UserAge = user.UserAge,
                            UserGender = user.UserGender,
                            UserRole = user.UserRole,
                            //به طور پیش فرض ناظر نمیتواند محموله ای را تغییر دهد باید شهرداری مجوز را بدهد
                            #region add permissons
                            //permissions = new List<UserPermissions>
                            //{
                            //    new UserPermissions()
                            //    {
                            //        Permission = "Permissions.Cargo.UpdateCargo",
                            //    },
                            //    new UserPermissions()
                            //    {
                            //        Permission = "Permissions.Cargo.DeleteCargo"
                            //    }
                            //}
                            #endregion
                        };
                        var UserParetnEmployeeId1 = user.UserParetnEmployeeId;
                        if (UserParetnEmployeeId1 != null)
                        {
                            user1.UserParetnEmployeeId = UserParetnEmployeeId1;
                        }

                        await base.AddAsync(user1, cancellationToken);
                        break;
                    }
                case UserRole.Contractor:
                    {
                        User user1 = new User()
                        {
                            CreateDate = DateTime.Today.ToShamsi(),
                            UserPasswordHash = user.UserPasswordHash,
                            UserName = user.UserName,
                            UserAge = user.UserAge,
                            UserGender = user.UserGender,
                            UserRole = user.UserRole,
                            // طبق سناریو این کاربر هم فقط میتواند فقط به قسمت هایی که نیاز به لاگین دارد دسترسی داشته باشد
                            #region add permissons
                            //permissions = new List<UserPermissions>
                            //{
                            //    new UserPermissions()
                            //    {
                            //        Permission = "Permissions.User.GetAll"
                            //    }
                            //}
                            #endregion
                        };
                        var UserParetnEmployeeId1 = user.UserParetnEmployeeId;
                        if (UserParetnEmployeeId1 != null)
                        {
                            user1.UserParetnEmployeeId = UserParetnEmployeeId1;
                        }
                        await base.AddAsync(user1, cancellationToken);
                        break;
                    }
                case UserRole.ExhibitorEmployee:
                    {
                        User user1 = new User()
                        {
                            CreateDate = DateTime.Today.ToShamsi(),
                            UserPasswordHash = user.UserPasswordHash,
                            UserName = user.UserName,
                            UserAge = user.UserAge,
                            UserGender = user.UserGender,
                            UserRole = user.UserRole,
                            UserPermissions = new List<UPermissions>
                            {
                                new UPermissions()
                                {
                                    Permission = "Permissions.Item.AddItem",

                                },
                                new UPermissions()
                                {
                                    Permission = "Permissions.Item.UpdateItem",
                                }
                            }
                        };
                        var UserParetnEmployeeId1 = user.UserParetnEmployeeId;
                        if (UserParetnEmployeeId1 != null)
                        {
                            user1.UserParetnEmployeeId = UserParetnEmployeeId1;
                        }
                        await base.AddAsync(user1, cancellationToken);
                        break;
                    }
                case UserRole.CarEmployee:
                    {
                        User user1 = new User()
                        {
                            CreateDate = DateTime.Today.ToShamsi(),
                            UserPasswordHash = user.UserPasswordHash,
                            UserName = user.UserName,
                            UserAge = user.UserAge,
                            UserGender = user.UserGender,
                            UserRole = user.UserRole,
                            // به دلیل ندانستن کار اصلی این کاربر یک مجوز دلخواه داده شده
                            UserPermissions = new List<UPermissions>
                            {
                                new UPermissions()
                                {
                                    Permission = "Permissions.Item.AddItem"
                                }
                            }
                        };
                        var UserParetnEmployeeId1 = user.UserParetnEmployeeId;
                        if (UserParetnEmployeeId1 != null)
                        {
                            user1.UserParetnEmployeeId = UserParetnEmployeeId1;
                        }
                        await base.AddAsync(user1, cancellationToken);
                        break;
                    }
                case UserRole.Customer:
                    {
                        User user1 = new User()
                        {
                            CreateDate = DateTime.Today.ToShamsi(),
                            UserPasswordHash = user.UserPasswordHash,
                            UserName = user.UserName,
                            UserAge = user.UserAge,
                            UserGender = user.UserGender,
                            UserRole = user.UserRole,
                            UserPermissions = new List<UPermissions>
                            {
                                new UPermissions()
                                {
                                    Permission = "Permissions.Order.AddToOrder"
                                },
                                new UPermissions()
                                {
                                    Permission = "Permissions.Order.DeleteFromOrder"
                                },
                                new UPermissions()
                                {
                                    Permission = "Permissions.Order.ShowOrder"
                                },
                                new UPermissions()
                                {
                                    Permission = "Permissions.Order.UpdateOrederDetailInOreder"
                                },
                            }
                        };

                        await base.AddAsync(user1, cancellationToken);
                        break;
                    }
                case UserRole.Admin:
                    {
                        User user1 = new User()
                        {
                            CreateDate = DateTime.Today.ToShamsi(),
                            UserPasswordHash = user.UserPasswordHash,
                            UserName = user.UserName,
                            UserAge = user.UserAge,
                            UserGender = user.UserGender,
                            UserRole = user.UserRole,
                            UserPermissions = new List<UPermissions>
                            {
                                new UPermissions()
                                {
                                    Permission = "Permissions.Admin.admin"
                                },
                            }
                        };
                        await base.AddAsync(user1, cancellationToken);
                        break;
                    }
            }
            return;
        }

        public async Task<User> Login(string firstName, string password, CancellationToken cancellationToken)
        {
            var passswordHash = SecurityHelper.GetSha256Hash(password);
            var user = await Table.Where(u => u.UserName == firstName && u.UserPasswordHash == passswordHash).SingleOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            var useId = user.Id;

            var ListPermissions = await DbContext.Set<UPermissions>().Where(u => u.userId == useId).ToListAsync();

            // authenticaiton successfo so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var claims = new ClaimsIdentity();

            foreach (var permisson in ListPermissions)
            {
                claims.AddClaims(new[]{
                    new Claim(Permissions.Permission,permisson.Permission.ToString()),
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    }); ;

            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.UserToken = tokenHandler.WriteToken(token);

            //The password will not be returned
            user.UserPasswordHash = null;
            user.UserPermissions = null;
            return user;
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
            return true;
            User User = await DbContext.Set<User>().Where(u => u.Id == id).SingleOrDefaultAsync(cancellationToken);
            var UserRole = User.UserRole;
            if (UserRole != UserRole.Supervisor)
            {
                return false;
            }
            User.UserPermissions = new List<UPermissions>
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
        }

        public async Task AllSupervisorChangePermissin(CancellationToken cancellationToken)
        {

            List<User> users = await DbContext.Set<User>().Where(u => u.UserRole == UserRole.Supervisor).ToListAsync();

            foreach (var user in users)
            {
                List<UPermissions> addPermission = new List<UPermissions>
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

                await DbContext.Set<UPermissions>().AddRangeAsync(addPermission, cancellationToken);
                await DbContext.SaveChangesAsync();
            }


        }
    }
}
