using APIV2.Models;

namespace APIV2.Service.Interfaces
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Task<ICollection<Game>> GetAllGames();
        //Task<User> Get(int userId);
    }
}
