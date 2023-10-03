using APIV2.Data;
using APIV2.Models;
using APIV2.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIV2.Service.Repositories
{
    public class BettingGameRepository : GenericRepository<BettingGame, GambleonContext>, IBettingGameRepository
    {
        public BettingGameRepository(GambleonContext context) 
            : base(context)
        {

        }
        public async Task<ICollection<BettingGame>> GetAllCurrentGames()
        {
            return await _context.Set<BettingGame>().Where(b => b.PlannedTime > DateTime.Now || b.beingPlayed == true)
                .Include(g => g.Game)
                .ThenInclude(c => c.Characters)
                .Include(b => b.BettingHistories)
                .ToListAsync();
        }
    }
}
