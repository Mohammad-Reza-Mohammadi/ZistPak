using Entities.User.Enum;
using Entities.User.UserProprety.EnumProperty;
using System.ComponentModel.DataAnnotations;

namespace presentation.Models
{
    public class SignupUserDto : IValidatableObject
    //با استفاده از IValidatableObject برخی از خطاهای بیزینیسی مربوط به dto را بررسی میکینم 
    //که مربوط به دیتابیس نیست، با استفاده از متدی از ان که در پایین ایمپلیمنت کردیم
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string phoneNumber { get; set; }
        public int Age { get; set; }
        public int? ParetnEmployeeId { get; set; }
        public UserRole Role { get; set; }
        public UserGender Gender { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserName.Equals("test", StringComparison.OrdinalIgnoreCase))
            {
                yield return new ValidationResult("نام کاربری نمیتواند test باشد", new[] { nameof(UserName) });
            }

            if (Password.Equals("123456"))
            {
                yield return new ValidationResult("رمز عبور نمیتواند 1234356 باشد", new[] { nameof(Password) });
            }

            // این شرط میتواند برای وبسایت های مربوط به استخدام باشد
            if (Gender == UserGender.Male && Age > 30)
            {
                yield return new ValidationResult("آقایان بالای 30 سال نمیتوانند ثبت نام کنند", new[] { nameof(Gender), nameof(Age) });
            }
        }
    }
}
