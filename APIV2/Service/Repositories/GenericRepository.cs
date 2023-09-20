using APIV2.Data;
using APIV2.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIV2.Service.Repositories
{
    public class GenericRepository<T, TDBContext> : IGenericRepository<T>
    where T : class
    where TDBContext : GambleonContext
    {

        protected GambleonContext _context;

        public GenericRepository(GambleonContext context)
        {
            this._context = context;
        }

        public async Task Insert(T obj)
        {
            _context.Add(obj);
            await Save();
        }
        public async Task Update(T obj)
        {
            _context.Update(obj);
            await Save();
        }
        public async Task Delete(int id)
        {
            var entityToDelete = _context.Set<T>().Find(id);
            _context.Remove(entityToDelete);
            await Save();
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<bool> entityExists(int id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            if (result != null)
            {
                _context.ChangeTracker.Clear();
                return true;
            }
            else
            {
                _context.ChangeTracker.Clear();
                return false;
            }

        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
