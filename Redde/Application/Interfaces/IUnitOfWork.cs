using Redde.Domain.Entities;

namespace Redde.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Company> Companies { get; }
        IRepository<Role> Roles { get; }

        Task<int> SaveChangesAsync();
    }
}
