using Data.Contracts;
using Data.Repositories;
using ECommerce.Utility;
using Entities.ModelsDto.User;
using Entities.User.Owned;
using Entities.User.UserProprety.EnumProperty;
using Entities.Useres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using presentation.Models;
using Services;
using Services.Services;
using System.Drawing;
using System.Net.Mime;
using System.Security.Claims;
using Utility.Exceptions;
using Utility.SwaggerConfig.Permissions;
using Utility.Utility;
using WebFramework.Api;
using WebFramework.Filters;
using static Utility.SwaggerConfig.Permissions.Permissions;
using User = Entities.Useres.User;

namespace presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiResultFilter]
    #region ApiResultFilter
    //با استفاده از این فیلتر خروجی کنترلر ها را به ApiResult تبدیل میکنیم 
    // توجه : خروجی هر کنترلری را از نوع ApiResult قرار ندهیم مانند کنترلر هایی
    // که خروجیشان ازنوع فایل هست چرا که به ApiResult قابل تبدیل شدن نیستند
    #endregion
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtSevice _jwtSevice;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;

        public UserController(IUserRepository userRepository, IJwtSevice jwtSevice, UserManager<User> userManager, RoleManager<Role> roleManager,
            SignInManager<User> signInManager)
        {
            this._userRepository = userRepository;
            this._jwtSevice = jwtSevice;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// برگرداندن تمامی کاربران
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(Policy = "GetAllUsersPolicy")]
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll(CancellationToken cancellationToken)
        {
            var users = await _userRepository.TableNoTracking.ToListAsync(cancellationToken);
            if (users.Equals(null))
            {
                return Content("کاربری یافت نشد !");
            }
            return Ok(users);
        }

        /// <summary>
        /// برگرداندن کاربر با استفاده از آیدی آن 
        /// </summary>
        /// <param name="id">آیدی کاربر</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(policy: "GetUserByIdPolicy")]
        [HttpGet("{id:int}")]
        public async Task<ApiResult<User>> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user2 = await userManager.FindByIdAsync(id.ToString());
            //var role = await roleManager.FindByNameAsync("Admin");
            //var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            //if (user == null)
            //    return NotFound(user);
            await _userRepository.UpdateLastLoginDateAsync(user2, cancellationToken);
            return user2;

        }

        /// <summary>
        /// This method is for User Login and get back a token in respons
        /// </summary>
        /// <param name="userName">The UserName of User</param>
        /// <param name="password">The Password of User</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        [AllowAnonymous]
        [HttpPost]
        public virtual async Task<ActionResult> Login([FromForm] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            if (!loginRequest.grant_type.Equals("password", StringComparison.OrdinalIgnoreCase))
                throw new AppException("AOuth flow is not password");
            var user = await userManager.FindByNameAsync(loginRequest.username);
            if (user == null)
                throw new BadRequestException("نام کاربری اشتباه است");
            var passwordIsValid = await userManager.CheckPasswordAsync(user, loginRequest.password);
            if (!passwordIsValid)
                throw new BadRequestException(" رمز عبور اشتباه است");
            var userToken = await _jwtSevice.GenerateAsync(user);
            return new JsonResult(userToken);

            //var user = await _userRepository.Login(userName, password, cancellationToken);
        }

        /// <summary>
        /// اضافه کردن کاربر(ریجستری)
        /// </summary>
        /// <param name="signupUserDto">مشخصات کاربر</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ApiResult<User>> SuignUp([FromForm] SignupUserDto signupUserDto, CancellationToken cancellationToken)
        {

            var addUserResponce = await _userRepository.AddUserAsync(signupUserDto, cancellationToken);
            if (signupUserDto.UserName.Equals("admin") && signupUserDto.Password.Equals("1234567") && signupUserDto.ParetnUsereId.Equals(null))
            {
                return Content($"حساب کاربری شما با موفقیت اضافه شد.");
            }
            else
            {
                if (addUserResponce.Equals(true))
                {
                    if (signupUserDto.Role.Equals(UserRole.Customer))
                    {
                        return Content($"{signupUserDto.UserName} عزیز به وبسایت ما خوش آمدید .");
                    }
                    else
                    {
                        if (signupUserDto.ParetnUsereId.Equals(null))
                        {
                            return Content($"{signupUserDto.UserName} عزیز به وبسایت ما خوش آمدید ، به منظور تایید حساب کابریتان به تایید مسؤلان مربوطه نیاز دارید ، پس از تایید حساب کابریتان به پنل کاربری خود دسترسی خواهید داشت .");
                        }
                        else
                        {
                            var userParent = await userManager.FindByIdAsync(signupUserDto.ParetnUsereId.ToString());
                            var parentName = userParent.UserName;
                            return Content($"{signupUserDto.UserName} عزیز به وبسایت ما خوش آمدید ، به منظور تایید حساب کابریتان به تایید {parentName}  نیاز دارید ، پس از تایید حساب کابریتان به پنل کاربری خود دسترسی خواهید داشت .");
                        }
                    }
                }
            }
            return Content("در روند ایجاد حساب کاربری شما مشکلی پیش آمده است! ");
        }

        /// <summary>
        /// آپدیت کردن مشخصات کاربر
        /// </summary>
        /// <param name="user">مشخصات جدید</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [ApiResultFilter]
        [HttpPut]
        public async Task<ApiResult> Update([FromForm] UpdateUserDto user, CancellationToken cancellationToken)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId1 = Convert.ToInt32(userId);

            var getUser = await _userRepository.FindById(userId1);

            #region update user

            getUser.UserName = user.UserName;
            getUser.UserAge = user.Age;
            getUser.PhoneNumber = user.PhoneNumber;
            getUser.Email = user.Email;
            // ElementAt(i)
            //getUser.UserAddresses.ElementAt(0).UserAddressTitle = user.AddressTitle;
            //getUser.UserAddresses.ElementAt(0).UserAddressCity = user.City;
            //getUser.UserAddresses.ElementAt(0).UserAddressTown = user.Town;
            //getUser.UserAddresses.ElementAt(0).UserAddressStreet = user.Street;
            //getUser.UserAddresses.ElementAt(0).UserAddressPostalCode = user.PostalCode;

            #endregion

            await _userRepository.UpdateUserAsync(getUser, userId1, user.ProductImage, cancellationToken);


            return Ok();
        }

        /// <summary>
        /// حذف کاربر با استفاده از آی دی آن
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindById(id);
            user.UserIsActive = false;
            await _userRepository.UpdateAsync(user, cancellationToken);

            return Content($"{user.UserName} باموفقیت حذف شد");
        }

        /// <summary>
        /// تغییر سطح دسترسی ناظر با استفاده از آی دی آن
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(Roles = "Municipality")]
        [HttpPost]
        public async Task<ApiResult> AddSoperviserPermissionById(int Id, CancellationToken cancellationToken)
        {
            bool Result = await _userRepository.ChangePermissinByID(Id, cancellationToken);

            if (Result == false)
            {
                return Content("شما قادر به تغییر مجوز های کاربر وارد شده نمی باشید .");
            }
            return Content("مجوزهای کاربر با موفقیت تغییر کرد .");

        }

        /// <summary>
        /// اضافه کردن سطح دسترسی به تمامی ناظران
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(Roles = "Municipality")]
        [HttpPost]
        public async Task<ActionResult> AddAllSoperviserPermission(CancellationToken cancellationToken)
        {
            await _userRepository.AllSupervisorChangePermissin(cancellationToken);
            return Ok();
        }

        /// <summary>
        /// فعال کردن کاربران با استفاده از آی دی(دسترسی ادمین)
        /// </summary>
        /// <param name="id">ای کاربر مورد نظر</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(policy: "ActiveUserAdminPolicy")]
        [HttpPost("{id:int}")]
        public async Task<ActionResult> ActiveUserAdmin(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            if (!await userManager.IsInRoleAsync(user, "Municipality"))
            {
                throw new BadRequestException("شما نمیتوانید مشخصات کاربر مورد نظر را تغییر دهید");
            }
            user.UserIsActive = true;
            await _userRepository.UpdateAsync(user, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// فعال کردن کاربران با استفاده از آی دی(دسترسی شهرداری)
        /// </summary>
        /// <param name="id">ای کاربر مورد نظر</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(policy: "ActiveUserMuniciaplityPolicy")]
        [HttpPost("{id:int}")]
        public async Task<ActionResult> ActiveUserMuniciaplity(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            if (await userManager.IsInRoleAsync(user, "Supervisor") || await userManager.IsInRoleAsync(user, "Contractor"))
            {
                user.UserIsActive = true;
                await _userRepository.UpdateAsync(user, cancellationToken);
                return Ok();
            }
            throw new BadRequestException("شما نمیتوانید مشخصات کاربر مورد نظر را تغییر دهید");
        }

        /// <summary>
        /// فعال کردن کاربران با استفاده از آی دی(دسترسی پیمانکار)
        /// </summary>
        /// <param name="id">ای کاربر مورد نظر</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [Authorize(policy: "ActiveUserCotractorPolicy")]
        [HttpPost("{id:int}")]
        public async Task<ActionResult> ActiveUserContractor(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            if (await userManager.IsInRoleAsync(user, "CarEmployee") || await userManager.IsInRoleAsync(user, "ExhibitorEmployee"))
            {
                user.UserIsActive = true;
                await _userRepository.UpdateAsync(user, cancellationToken);
                return Ok();
            }
            throw new BadRequestException("شما نمیتوانید مشخصات کاربر مورد نظر را تغییر دهید");
        }

    }
}

