using Entities.User.Enum;
using Entities.User.UserProprety.EnumProperty;

namespace presentation.Models
{
    public class SignupUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string phoneNumber { get; set; }
        public int Age { get; set; }
        public int? ParetnEmployeeId { get; set; }
        public UserRole Role { get; set; }
        public UserGender Gender { get; set; }
    }
}
