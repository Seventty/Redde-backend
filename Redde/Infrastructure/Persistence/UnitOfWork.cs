using Redde.Application.Interfaces;
using Redde.Domain.Entities;
using Redde.Infraestructure.Persistence;
using Redde.Infrastructure.Repositories;

namespace Redde.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRepository<User> Users { get; private set; }
        public IRepository<Company> Companies { get; private set; }
        public IRepository<Role> Roles { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Users = new Repository<User>(_context);
            Companies = new Repository<Company>(_context);
            Roles = new Repository<Role>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
