using Microsoft.EntityFrameworkCore;
using Redde.Application.Interfaces;
using Redde.Infraestructure.Persistence;
using System.Linq.Expressions;

namespace Redde.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException($"{typeof(T).Name} with id {id} not found.");
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _dbSet.Remove(entity);
        }
    }
}
