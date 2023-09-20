using APIV2.Models;

namespace APIV2.Service.Interfaces
{
    public interface IBettingGameRepository : IGenericRepository<BettingGame>
    {
        Task<ICollection<BettingGame>> GetAllCurrentGames();
    }
}
