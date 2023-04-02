using Entities.User.Enum;
using Entities.User.UserProprety.EnumProperty;

namespace presentation.Models
{
    public class SignupUserDto
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string AddressTitle { get; set; }
        public int? ParetnEmployeeId { get; set; }
        public Role Role { get; set; }
        public Gender Gender { get; set; }
    }
}
