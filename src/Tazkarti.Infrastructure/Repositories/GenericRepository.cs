using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tazkarti.Domain.Interfaces;
using Tazkarti.Infrastructure.Data;

namespace Tazkarti.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly TazkartiDbContext _context;

    public GenericRepository(TazkartiDbContext context)
    {
        _context = context;
    }

    public virtual async Task<T?> GetByIdAsync(object id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(string? include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (!string.IsNullOrEmpty(include))
        {
            foreach (var includeProp in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        return await query.ToListAsync();
    }

    public virtual async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> FindByIdAsync(Expression<Func<T, bool>> predicate, string? include = null)
    {

		IQueryable<T> query = _context.Set<T>();

		if (!string.IsNullOrEmpty(include))
		{
			foreach (var includeProp in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProp);
			}
		}

		return await query.Where(predicate).FirstOrDefaultAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public virtual void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public virtual void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task<IReadOnlyList<T>> GetAllWithIdAsync(Expression<Func<T, bool>> predicate,string? include = null)
    {

        IQueryable<T> query = _context.Set<T>();

		if (!string.IsNullOrEmpty(include))
		{
			foreach (var includeProp in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProp);
			}
		}

		return await _context.Set<T>().Where(predicate).ToListAsync();
    }
}
