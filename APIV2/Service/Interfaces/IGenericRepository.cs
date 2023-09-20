namespace APIV2.Service.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<ICollection<T>> GetAllAsync();
        Task<T> GetById(int id);
        Task<bool> entityExists(int id);
        Task Insert(T obj);
        Task Update(T obj);
        Task Delete(int id);
        Task Save();
    }
}
