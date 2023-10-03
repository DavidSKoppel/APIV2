using APIV2.Data;
using APIV2.Dtos.User;
using APIV2.Models;
using APIV2.Service.Interfaces;
using APIV2.Service.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UserController : Controller
    {
        //private readonly GambleonContext _context;
        private IUserRepository _userRepository; 
        private readonly IConfiguration _configuration;
        public UserController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        /*public UserController(GambleonContext context)
        {
            _context = context;
        }*/
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User()
            {
                Username = userDto.Username,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Active = true,
                DateOfBirth = userDto.dateOfBirth,
                PhoneNumber = userDto.PhoneNumber,
                UserTypeId = 1,
                Address = new Address()
                {
                    Address1 = userDto.Address.Address1,
                    PostalCode = userDto.Address.PostalCode
                }
            };

            //kode til hash
            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            try
            {
                await _userRepository.Insert(user);
            }
            catch (Exception e)
            {
                var eNumber = e.InnerException as SqlException;
                if (eNumber.Number == 2627)
                {
                    return StatusCode(406, "Email already in use");
                }
                else
                {
                    ModelState.AddModelError("", e.GetBaseException().Message);
                    return StatusCode(500, ModelState);
                }
            }

            return CreatedAtAction("GetUsers", user);
        }
        [HttpGet("PhoneNumber")]
        public async Task<IActionResult> GetByPhone(int phoneNumber)
        {
            var user = await _userRepository.GetUserByPhoneNumber(phoneNumber);

            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpGet("Email")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);

            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpGet("GetUsersWithAllHistory"), Authorize(Roles = "Admin, Internal")]
        public async Task<IActionResult> GetUsersWithAllHistory()
        {
            var users = await _userRepository.GetUsersWithAllBettingHistory();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);

        }

        [HttpGet("UserWalletAndTransactions"), Authorize(Roles = "Admin, Internal")]
        public async Task<IActionResult> GetUserWalletAndTrans()
        {
            var users = await _userRepository.GetUserWalletAndTrans();

            if (users == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpPost("UserLogin")]
        public async Task<IActionResult> LoginUser(LoginUserDto user)
        {
            bool loginSuccess;
            try
            {
                loginSuccess = await _userRepository.CheckIfUserExistByEmail(user.email, user.password);
            }
            catch(Exception e)
            {
                return StatusCode(404, "Username or password is wrong");
            }
            if (loginSuccess)
            {
                User userByEmail;
                try
                {
                   userByEmail = await _userRepository.GetUserByEmail(user.email);
                }
                catch(Exception e)
                {
                    return BadRequest(e);
                }
                if (userByEmail.Active == true)
                {
                    LoginSuccessUserDto login = new LoginSuccessUserDto();
                    login.id = userByEmail.Id;
                    login.email = userByEmail.Email;
                    login.username = userByEmail.Username;
                    login.userType1 = userByEmail.UserType.UserType1;
                    string token = CreateToken(user.email, userByEmail.UserType.UserType1);
                    var obj = new { login, token };
                    return Ok(obj);
                }
                else
                {
                    return StatusCode(423, "User inactive");
                }
            }
            return StatusCode(418, "is Bed");
        }
        
        private string CreateToken(string email, string userType)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, userType)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        
        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetUser(int id)
        {
            if (!await _userRepository.entityExists(id))
                return NotFound();
            var user = await _userRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        //api/user
        [HttpGet, Authorize(Roles = "Admin, Internal")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);
        }

        //api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User createUser)
        {
            if (createUser == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userRepository.Insert(createUser);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return CreatedAtAction("Getuser", new { id = createUser.Id }, createUser);
        }


        //api/users/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updateUser)
        {
            if (updateUser == null)
            {
                return BadRequest(ModelState);
            }
            if (id != updateUser.Id)
            {
                return BadRequest(ModelState);
            }
            if (!await _userRepository.entityExists(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userRepository.Update(updateUser);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        // DELETE: api/users/3
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!await _userRepository.entityExists(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _userRepository.Delete(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}