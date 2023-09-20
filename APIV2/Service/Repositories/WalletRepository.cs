using APIV2.Data;
using APIV2.Models;
using APIV2.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIV2.Service.Repositories
{
    public class WalletRepository : GenericRepository<Wallet, GambleonContext>, IWalletRepository
    {
        public WalletRepository(GambleonContext context)
            : base(context)
        {

        }
        public async Task<Wallet> GetWalletByUserId(int userId)
        {
            return await _context.Wallets.Where(e => e.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}
