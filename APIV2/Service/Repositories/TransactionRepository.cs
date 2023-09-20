using APIV2.Data;
using APIV2.Models;
using APIV2.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIV2.Service.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction, GambleonContext>, ITransactionRepository
    {
        public TransactionRepository(GambleonContext context)
            : base(context)
        {

        }

        public async Task<ICollection<Transaction>> GetTransactionsByUserId(int userId)
        {
            return await _context.Transactions.Where(e => e.Wallet.UserId == userId)
                .ToListAsync();
        }
    }
}
