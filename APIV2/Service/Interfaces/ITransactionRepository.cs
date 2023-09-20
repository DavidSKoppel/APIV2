using APIV2.Models;

namespace APIV2.Service.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<ICollection<Transaction>> GetTransactionsByUserId(int userId);
    }
}
