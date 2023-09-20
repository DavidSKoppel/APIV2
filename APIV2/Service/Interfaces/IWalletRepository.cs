using APIV2.Models;

namespace APIV2.Service.Interfaces
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<Wallet> GetWalletByUserId(int userId);
    }
}
