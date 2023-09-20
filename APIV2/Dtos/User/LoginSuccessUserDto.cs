using APIV2.Models;

namespace APIV2.Dtos.User
{
    public class LoginSuccessUserDto
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string userType1 { get; set; }
    }
}
