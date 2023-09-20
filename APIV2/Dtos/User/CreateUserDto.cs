using APIV2.Dtos.Address;
using APIV2.Models;

namespace APIV2.Dtos.User
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public int PhoneNumber { get; set; }
        public DateTime dateOfBirth { get; set; }

        public string Username { get; set; }

        public AddressDto Address { get; set; }
    }
}
