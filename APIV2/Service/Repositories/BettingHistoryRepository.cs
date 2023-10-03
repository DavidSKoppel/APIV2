using APIV2.Data;
using APIV2.Models;
using APIV2.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIV2.Service.Repositories
{
    public class BettingHistoryRepository : GenericRepository<BettingHistory, GambleonContext>, IBettingHistoryRepository
    {
        public BettingHistoryRepository(GambleonContext context)
            : base(context)
        {

        }

        public async Task<ICollection<BettingHistory>> GetBettingHistoriesByBettingGameId(int betGameId)
        {
            return await _context.BettingHistories.Where(e => e.BettingGameId == betGameId)
                .ToListAsync();
        }

        public async Task<ICollection<BettingHistory>> GetBettingHistoriesByUserId(int userId)
        {
            return await _context.BettingHistories.Where(b => b.Wallet.UserId == userId)
                .Include(e => e.BettingGame.Game.Characters)
                .ToListAsync();
        }

        public async Task<ICollection<BettingHistory>> GetByUserIdAndBetGameId(int userId, int betId)
        {
            return await _context.BettingHistories.Where(e => e.Wallet.UserId == userId && e.BettingGameId == betId)
                .ToListAsync();
        }
    }
}