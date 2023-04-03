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

        public async Task AddAsync(User user, string password, Role Role, CancellationToken cancellationToken)
        {
            var passswordHash = SecurityHelper.GetSha256Hash(password);
            user.HashPassword = passswordHash;

            //به طور پیش فرض افراد فقط میتوانند به کنترلر هایی دسترسی داشته باشند که فقط نیاز به لاگین دارد
            // give permission
            switch (Role)
            {
                case Role.Supervisor:
                    {
                        User user1 = new User()
                        {
                            CreateDate = DateTime.Today.ToShamsi(),
                            HashPassword = user.HashPassword,
                            FullName = user.FullName,
                            Age = user.Age,
                            Addresses = user.Addresses,
                            Gender = user.Gender,
                            Role = user.Role,
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
                        var peId = user.ParetnEmployeeId;
                        if (peId != null)
                        {
                            user.ParetnEmployeeId = user1.ParetnEmployeeId.Value;
                        }
                        await base.AddAsync(user1, cancellationToken);
                        break;
                    }
                case Role.Contractor:
                    {
                        User user1 = new User()
                        {
                            CreateDate = DateTime.Today.ToShamsi(),
                            HashPassword = user.HashPassword,
                            FullName = user.FullName,
                            Age = user.Age,
                            Addresses = user.Addresses,
                            Gender = user.Gender,
                            Role = user.Role,
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
                        var peId = user.ParetnEmployeeId;
                        if (peId != null)
                        {
                            user.ParetnEmployeeId = user1.ParetnEmployeeId.Value;
                        }
                        await base.AddAsync(user1, cancellationToken);
                        break;
                    }
                case Role.ExhibitorEmployee:
                    {
                        User user1 = new User()
                        {
                            CreateDate = DateTime.Today.ToShamsi(),
                            HashPassword = user.HashPassword,
                            FullName = user.FullName,
                            Age = user.Age,
                            Addresses = user.Addresses,
                            Gender = user.Gender,
                            Role = user.Role,
                            permissions = new List<UPermissions>
                            {
                                new UPermissions()
                                {
                                    Permission = "Permissions.Item.AddItem",
                                    municiaplityId =4,

                                },
                                new UPermissions()
                                {
                                    Permission = "Permissions.Item.UpdateItem",
                                    municiaplityId =4,
                                }
                            }
                        };
                        var peId = user.ParetnEmployeeId;
                        if (peId != null)
                        {
                            user.ParetnEmployeeId = user1.ParetnEmployeeId.Value;
                        }
                        await base.AddAsync(user1, cancellationToken);
                        break;
                    }
                case Role.CarEmployee:
                    {
                        User user1 = new User()
                        {
                            CreateDate = DateTime.Today.ToShamsi(),
                            HashPassword = user.HashPassword,
                            FullName = user.FullName,
                            Age = user.Age,
                            Addresses = user.Addresses,
                            Gender = user.Gender,
                            Role = user.Role,
                            // به دلیل ندانستن کار اصلی این کاربر یک مجوز دلخواه داده شده
                            permissions = new List<UPermissions>
                            {
                                new UPermissions()
                                {
                                    municiaplityId =4,
                                    Permission = "Permissions.Item.AddItem"
                                }
                            }
                        };
                        var peId = user.ParetnEmployeeId;
                        if (peId != null)
                        {
                            user.ParetnEmployeeId = user1.ParetnEmployeeId.Value;
                        }
                        await base.AddAsync(user1, cancellationToken);
                        break;
                    }
                case Role.Customer:
                    {
                        User user1 = new User()
                        {
                            CreateDate = DateTime.Today.ToShamsi(),
                            HashPassword = user.HashPassword,
                            FullName = user.FullName,
                            Age = user.Age,
                            Addresses = user.Addresses,
                            Gender = user.Gender,
                            Role = user.Role,
                            //permissions = new List<UserPermissions>
                            //{
                            //    new UserPermissions()
                            //    {
                            //        Permission = "Permissions.Item.AddItem"
                            //    },
                            //}
                        };
                        var peId = user.ParetnEmployeeId;
                        if (peId != null)
                        {
                            user.ParetnEmployeeId = user1.ParetnEmployeeId.Value;
                        }
                        await base.AddAsync(user1, cancellationToken);
                        break;
                    }
                case Role.Admin:
                    {
                        User user1 = new User()
                        {
                            CreateDate = DateTime.Today.ToShamsi(),
                            HashPassword = user.HashPassword,
                            FullName = user.FullName,
                            Age = user.Age,
                            Addresses = user.Addresses,
                            Gender = user.Gender,
                            Role = user.Role,
                            permissions = new List<UPermissions>
                            {
                                new UPermissions()
                                {
                                    municiaplityId =4,
                                    Permission = "Permissions.User.GetAll"
                                },
                                new UPermissions()
                                {
                                    municiaplityId =4,
                                    Permission = "Permissions.User.Delete"
                                },
                                new UPermissions()
                                {
                                    municiaplityId =4,
                                    Permission = "Permissions.User.GetUserById"
                                },
                                new UPermissions()
                                {
                                    municiaplityId =4,
                                    Permission = "Permissions.Item.AddItem"
                                },
                                new UPermissions()
                                {
                                    municiaplityId =4,
                                    Permission = "Permissions.Item.UpdateItem"
                                },
                                new UPermissions()
                                {
                                    municiaplityId =4,
                                    Permission = "Permissions.Item.Delete"
                                },
                                new UPermissions()
                                {
                                    municiaplityId =4,
                                    Permission = "Permissions.Cargo.AddCargo"
                                },
                                new UPermissions()
                                {
                                    municiaplityId =4,
                                    Permission = "Permissions.Cargo.UpdateCargo"
                                },
                                new UPermissions()
                                {
                                    municiaplityId =4,
                                    Permission = "Permissions.Cargo.DeleteCargo"
                                }, 
                                new UPermissions()
                                {
                                    municiaplityId =4,
                                    Permission = "Permissions.Municipality.AddSoperviserPermissionById"
                                },
                                new UPermissions()
                                {
                                    municiaplityId =4,
                                    Permission = "Permissions.Municipality.AddAllSoperviserPermission"
                                },      
                            }
                        };
                        var peId = user.ParetnEmployeeId;
                        if (peId != null)
                        {
                            user.ParetnEmployeeId = user1.ParetnEmployeeId.Value;
                        }
                        await base.AddAsync(user1, cancellationToken);
                        break;
                    }
            }
            return;
        }

        public async Task<User> Login(string firstName, string password, CancellationToken cancellationToken)
        {
            var passswordHash = SecurityHelper.GetSha256Hash(password);
            var user = await Table.Where(u => u.FullName.FirstName == firstName && u.HashPassword == passswordHash).SingleOrDefaultAsync();
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
            user.Token = tokenHandler.WriteToken(token);

            //The password will not be returned
            user.HashPassword = null;
            user.permissions = null;
            return user;
        }

        public async Task UpdateUserAsync(User user, int userId, IFormFile userImageFile, CancellationToken cancellationToken)
        {
            string ImagPath = UserImageExtension.ImgeToString(userImageFile);
            user.Image = ImagPath;
            await base.UpdateAsync(user, cancellationToken);
            return;

        }

        // اول لیست مجوز ها را از دیتا بیس بگیر 



    }
}

#region roley
//new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
//new Claim(ClaimTypes.Role , user.Role.ToString()),
#endregion
#region Claim or Policy
//باید به در قسمت اضافه کردن policy ها یک policy جدید اضافه کنیم
//Claim Base (in Sevice.Authorization add section)
//new Claim("AccessAlluser" , user.AccsessAllUser.ToString()),////user.AccsessAllUser==bool property in user table
#endregion