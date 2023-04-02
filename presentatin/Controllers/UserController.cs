using Data.Contracts;
using ECommerce.Utility;
using Entities.User.Owned;
using Entities.Useres;
using Entities.Useres.UserProprety.EnumProperty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using presentation.Models;
using System.Drawing;
using System.Security.Claims;

namespace presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        // برای دسترسی علاوه بر نقش باید iaActive هم چک بشود
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return Ok(users);
        }

        [Authorize(Roles ="Admin")]
        //[Authorize(Policy ="GetAllUser")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();
            return user;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SuignUp([FromForm] SignupUserDto signupUserDto, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                FullName = new FullName
                {
                    FirstName = signupUserDto.FirstName,
                },
                Age = signupUserDto.Age,
                Addresses = new List<Address>
                {
                    new Address()
                    {
                        AddressTitle = signupUserDto.AddressTitle,
                        City = signupUserDto.City,
                    }

                },
                Gender = signupUserDto.Gender,
                Role = signupUserDto.Role,
                permissionLevel = PermissionLevel.level1,
                CreateDate = DateTime.Today.ToShamsi(),                
            };
            var peId = signupUserDto.ParetnEmployeeId;
            if (peId == null)
            {
            await userRepository.AddAsync(user, signupUserDto.Password, cancellationToken);
            }
            else
            {
                user.ParetnEmployeeId = signupUserDto.ParetnEmployeeId.Value;
            }
            await userRepository.AddAsync(user, signupUserDto.Password, cancellationToken);


            return Content($"{signupUserDto.FirstName} : با موفقیت اضافه شد ");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> Login(string FirstName, string password, CancellationToken cancellationToken)
        {
            var user = await userRepository.Login(FirstName, password, cancellationToken);
            if (user == null)
                return Content($"{FirstName} یافت نشد");
            return user;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> Update([FromForm] UpdateUserDto user, CancellationToken cancellationToken)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId1 =Convert.ToInt32(userId);
            var GetUser = await userRepository.GetByIdAsync(cancellationToken, userId1);

            #region update user

            GetUser.FullName.FirstName = user.FirstName;
            GetUser.FullName.LastName = user.LasteName;
            GetUser.Age = user.Age;
            GetUser.PhoneNumber = user.PhoneNumber;
            GetUser.Email = user.Email;
            // ElementAt(i)
            GetUser.Addresses.ElementAt(0).AddressTitle = user.AddressTitle;
            GetUser.Addresses.ElementAt(0).City = user.City;
            GetUser.Addresses.ElementAt(0).Town = user.Town;
            GetUser.Addresses.ElementAt(0).Street = user.Street;
            GetUser.Addresses.ElementAt(0).PostalCode = user.PostalCode;

            #endregion

            await userRepository.UpdateUserAsync(GetUser, userId1, user.ProductImage, cancellationToken);


            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            user.IsActive = false;
            await userRepository.UpdateAsync(user,cancellationToken);

            return Ok();
        }
    }
}
