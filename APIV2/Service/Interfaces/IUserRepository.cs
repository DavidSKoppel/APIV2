using APIV2.Models;
using System.ComponentModel;

namespace APIV2.Service.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<ICollection<User>> GetUsersWithAllInfo();
        Task<bool> CheckIfUserExistByEmail(string email, string password);
        Task<User> GetUserWithAllInfo(int userId);
        Task PutNewUserPassword(int userId, byte[] passwordHash, byte[] passwordSalt);
        Task PutNewUserAddress(int userId, int address);
        Task PutNewUserInfo(int userId, string firstName, string lastName, string email, int phoneNumber);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByPhoneNumber(int phoneNumber);
    }
}
