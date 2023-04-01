using Entities.User.Enum;
using Entities.User.Owned;

namespace presentation.Models
{
    public class CustomerDto
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string AddressTitle { get; set; }
        public Gender Gender { get; set; }

    }

}


