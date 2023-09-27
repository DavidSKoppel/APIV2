using APIV2.Data;
using APIV2.Models;
using APIV2.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace APIV2.Service.Repositories
{
    public class UserRepository : GenericRepository<User, GambleonContext>, IUserRepository
    {
        public UserRepository(GambleonContext context)
            : base(context)
        {

        }

        public async Task<User> GetUserWithAllInfo(int userId)
        {
            return await _context.Users.Where(c => c.Id == userId)
                //.Include(c => c.Address)
                //.Include(s => s.Address.PostalCode)
                .FirstOrDefaultAsync();
        }

        public async Task PutNewUserPassword(int userId, byte[] passwordHash, byte[] passwordSalt)
        {
            var user = new User() { Id = userId, PasswordHash = passwordHash, PasswordSalt = passwordSalt };
            _context.Users.Attach(user);
            _context.Entry(user).Property(x => x.PasswordHash).IsModified = true;
            _context.Entry(user).Property(x => x.PasswordSalt).IsModified = true;
            await _context.SaveChangesAsync();
        }

        public async Task PutNewUserAddress(int userId, int address)
        {
            var user = new User() { Id = userId, AddressId = address };
            _context.Users.Attach(user);
            _context.Entry(user).Property(x => x.AddressId).IsModified = true;
            await _context.SaveChangesAsync();
        }

        public async Task PutNewUserInfo(int userId, string firstName, string lastName, string email, int phoneNumber)
        {
            var user = new User() { Id = userId, FirstName = firstName, LastName = lastName, Email = email, PhoneNumber = phoneNumber };
            _context.Users.Attach(user);
            _context.Entry(user).Property(x => x.FirstName).IsModified = true;
            _context.Entry(user).Property(x => x.LastName).IsModified = true;
            _context.Entry(user).Property(x => x.Email).IsModified = true;
            _context.Entry(user).Property(x => x.PhoneNumber).IsModified = true;
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<User>> GetUsersWithAllInfo()
        {
            return await _context.Set<User>()
                //.Include(c => c.Address)
                //.Include(s => s.Address.PostalCode)
                .ToListAsync();
        }

        public async Task<bool> CheckIfUserExistByEmail(string email, string password)
        {
            User user = _context.Users.Where(e => e.Email == email).FirstOrDefault();
            if(user != null)
            {
                bool verified = VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
                if (verified)
                {
                    return true;
                }
            }
            return false; 
        }

        private bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hash);
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.Where(c => c.Email == email)
                .Include(c => c.UserType)
                .Include(c => c.Address)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByPhoneNumber(int phoneNumber)
        {
            return await _context.Users.Where(c => c.PhoneNumber == phoneNumber)
                .Include(c => c.UserType)
                .Include(c => c.Address)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<User>> GetUserWalletAndTrans()
        {
            return await _context.Set<User>()
                .Include(c => c.Address)
                //.Include(c => c.UserType)
                //.Include(s => s.Address.PostalCode)
                .Include(w => w.Wallets)
                .ThenInclude(wallet => wallet.Transactions)
                .ToListAsync();
        }

        public async Task<ICollection<User>> GetUsersWithAllBettingHistory()
        {
            return await _context.Set<User>()
                .Include(u => u.Wallets)
                .ThenInclude(wallet => wallet.BettingHistories)
                .ThenInclude(bets => bets.BettingGame)
                .ThenInclude(betgame => betgame.Game)
                .ToListAsync();
        }
    }
}
