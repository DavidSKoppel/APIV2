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
            return await _context.Set<BettingGame>().Where(b => b.PlannedTime > DateTime.Now)
                .Include(e => e.Game)
                //.Include(c => c.Address)
                //.Include(s => s.Address.PostalCode)
                .ToListAsync();
        }
    }
}
