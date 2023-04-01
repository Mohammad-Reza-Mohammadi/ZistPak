using Data.Contracts;
using ECommerce.Utility;
using Entities.User.Owned;
using Entities.Useres;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using presentation.Models;
using System.Drawing;

namespace presentation.Controllers
{
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

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost]
        public async Task<ActionResult> SuignUp([FromBody] SignupUserDto signupUserDto, CancellationToken cancellationToken)
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
                CreateDate = DateTime.Today.ToShamsi(),
            };


            await userRepository.AddAsync(user, signupUserDto.Password, cancellationToken);

            return Content($"{signupUserDto.FirstName} : با موفقیت اضافه شد ");

        }

        // نیاز به احراز هویت
        [HttpPut]
        public async Task<ActionResult> Update(int id,[FromForm] UpdateUserDto user, CancellationToken cancellationToken)
        {
            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);
        
            updateUser.FullName.FirstName = user.FirstName;
            updateUser.FullName.LastName = user.LasteName;
            updateUser.Age = user.Age;
            updateUser.PhoneNumber = user.PhoneNumber;
            updateUser.Email = user.Email;
            updateUser.Addresses.ElementAt(0).AddressTitle = user.AddressTitle;
            updateUser.Addresses.ElementAt(0).City = user.City;
            updateUser.Addresses.ElementAt(0).Town = user.Town;
            updateUser.Addresses.ElementAt(0).Street = user.Street;
            updateUser.Addresses.ElementAt(0).PostalCode = user.PostalCode;

            await userRepository.UpdateUserAsync(updateUser, updateUser.Id, user.ProductImage, cancellationToken);


            return Ok();
        }

        // با کنسلیشن توکن دیگر ریکوئستهای اضافه پردازش نمیشوند
        [HttpDelete]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            user.IsActive = false;
            await userRepository.UpdateAsync(user,cancellationToken);

            return Ok();
        }
    }
}
