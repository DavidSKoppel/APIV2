using APIV2.Data;
using APIV2.Models;
using APIV2.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIV2.Service.Repositories
{
    public class GameRepository : GenericRepository<Game, GambleonContext>, IGameRepository
    {
        public GameRepository(GambleonContext context)
            : base(context)
        {

        }

        public async Task<ICollection<Game>> GetAllGames()
        {
            return await _context.Set<Game>()
                //.Include(c => c.Address)
                //.Include(s => s.Address.PostalCode)
                .ToListAsync();
        }

        public async Task<Game> GetRandomGameWithCharacters()
        {
            Random rng = new Random();
            List<Game> listOfGames = _context.Games.ToList();

            Game game = listOfGames[rng.Next(0, listOfGames.Count)];
            
            return await _context.Games.Where(c => c.Id == game.Id)
                .Include(c => c.Characters)
                .FirstOrDefaultAsync();
        }
    }
}
