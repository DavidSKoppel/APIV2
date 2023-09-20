using APIV2.Models;

namespace APIV2.Service.Interfaces
{
    public interface IBettingHistoryRepository : IGenericRepository<BettingHistory>
    {
        Task<ICollection<BettingHistory>> GetBettingHistoriesByUserId(int userId);
    }
}
