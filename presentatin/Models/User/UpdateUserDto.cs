using Entities.User.Owned;

namespace presentation.Models
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; }
        public string LasteName { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public IFormFile ProductImage { get; set; }
        public string AddressTitle { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}
