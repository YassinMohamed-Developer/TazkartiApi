using System.Collections.Concurrent;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Interfaces;
using Tazkarti.Infrastructure.Data;

namespace Tazkarti.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TazkartiDbContext _context;
    private readonly ConcurrentDictionary<string, object> _repositories = new();

    public UnitOfWork(TazkartiDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T).Name;

        return (IGenericRepository<T>)_repositories.GetOrAdd(type, _ => new GenericRepository<T>(_context));
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
