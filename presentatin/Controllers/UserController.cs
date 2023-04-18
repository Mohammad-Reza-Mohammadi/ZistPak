using Data.Contracts;
using Data.Repositories;
using ECommerce.Utility;
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
using Services.Services;
using System.Drawing;
using System.Net.Mime;
using System.Security.Claims;
using Utility.Exceptions;
using Utility.SwaggerConfig.Permissions;
using WebFramework.Api;
using WebFramework.Filters;
using static Utility.SwaggerConfig.Permissions.Permissions;
using User = Entities.Useres.User;

namespace presentation.Controllers
{
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

        public UserController(IUserRepository userRepository,IJwtSevice jwtSevice,UserManager<User> userManager, RoleManager<Role> roleManager,
            SignInManager<User> signInManager)
        {
            this._userRepository = userRepository;
            this._jwtSevice = jwtSevice;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }


        //[PermissionAuthorize(Permissions.User.GetAll, Admin.admin)]
        //[AllowAnonymous]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll(CancellationToken cancellationToken)
        {
            var users = await _userRepository.TableNoTracking.ToListAsync(cancellationToken);
            if (users != null)
            {
                return NotFound(users);
            }
            return Ok(users);
        }

        //[PermissionAuthorize(Permissions.User.GetUserById, Admin.admin)]
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ApiResult<User>> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user2 = await userManager.FindByIdAsync(id.ToString());
            var role = await roleManager.FindByNameAsync("Admin");
            //var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            //if (user == null)
            //    return NotFound(user);
            return user2;

        }
   

        [AllowAnonymous]
        [HttpPost]
        public async Task<ApiResult<string>> Login(string userName, string password, CancellationToken cancellationToken)
        {
            //var user = await _userRepository.Login(userName, password, cancellationToken);
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
                throw new BadRequestException("نام کاربری اشتباه است");
            var passwordIsValid = await userManager.CheckPasswordAsync(user, password);
            if (!passwordIsValid)
                return Content(" رمز عبور اشتباه است");
            var userWithToken =await _jwtSevice.GenerateAsync(user,cancellationToken);
            return Ok(userWithToken);
        }




        [AllowAnonymous]
        [HttpPost]
        public async Task<ApiResult<User>> SuignUp([FromForm]SignupUserDto signupUserDto, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                UserName = signupUserDto.UserName,
                UserPhoneNumber = signupUserDto.phoneNumber,
                UserAge = signupUserDto.Age,
                UserGender = signupUserDto.Gender,
                Email = signupUserDto.Email,
                CreateDate = DateTime.Now.ToShamsi(),
            };
            var result = await userManager.CreateAsync(user ,signupUserDto.Password);

            var result2 = await roleManager.CreateAsync(new Role
            {
                Name = "Admin",
                Description = "admin role"
            });

            var result3 = await userManager.AddToRoleAsync(user, "Admin");
            return user;

            #region
            //var parentEployeeId = signupUserDto.ParetnEmployeeId;
            //if (parentEployeeId == null)
            //{
            //    await _userRepository.AddUserAsync(user, signupUserDto.Password, cancellationToken);
            //}
            //else
            //{
            //    user.UserParetnEmployeeId = signupUserDto.ParetnEmployeeId;
            //    await _userRepository.AddUserAsync(user, signupUserDto.Password, cancellationToken);
            //}

            //return Content($"{signupUserDto.UserName}: عزیز به وبسایت ما خوش آمدید");

            #endregion
        }







        [ApiResultFilter]
        [HttpPut]
        public async Task<ApiResult> Update([FromForm] UpdateUserDto user, CancellationToken cancellationToken)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId1 = Convert.ToInt32(userId);

            var getUser = await _userRepository.GetByIdAsync(cancellationToken, userId1);

            #region update user

            getUser.UserName = user.UserName;
            getUser.UserAge = user.Age;
            getUser.UserPhoneNumber = user.PhoneNumber;
            getUser.UserEmail = user.Email;
            // ElementAt(i)
            getUser.UserAddresses.ElementAt(0).UserAddressTitle = user.AddressTitle;
            getUser.UserAddresses.ElementAt(0).UserAddressCity = user.City;
            getUser.UserAddresses.ElementAt(0).UserAddressTown = user.Town;
            getUser.UserAddresses.ElementAt(0).UserAddressStreet = user.Street;
            getUser.UserAddresses.ElementAt(0).UserAddressPostalCode = user.PostalCode;

            #endregion

            await _userRepository.UpdateUserAsync(getUser, userId1, user.ProductImage, cancellationToken);


            return Ok();
        }

        //[PermissionAuthorize(Permissions.User.Delete, Admin.admin)]
        [HttpDelete("{id:int}")]
        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            user.UserIsActive = false;
            await _userRepository.UpdateAsync(user, cancellationToken);

            return Content($"{user.UserName} باموفقیت حذف شد");
        }

        //[PermissionAuthorize(Permissions.User.AddSoperviserPermissionById, Admin.admin)]
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

        //[PermissionAuthorize(Permissions.User.AddAllSoperviserPermission, Admin.admin)]
        [HttpPost]
        public async Task<ActionResult> AddAllSoperviserPermission(CancellationToken cancellationToken)
        {
            await _userRepository.AllSupervisorChangePermissin(cancellationToken);
            return Ok();
        }


    }
}
