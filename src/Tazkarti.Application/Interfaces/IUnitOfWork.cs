using Tazkarti.Domain.Interfaces;

namespace Tazkarti.Application.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IGenericRepository<T> Repository<T>() where T : class;
    Task<int> CompleteAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
