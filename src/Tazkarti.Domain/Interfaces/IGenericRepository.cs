
using System.Linq.Expressions;

namespace Tazkarti.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(object id);
    Task<IReadOnlyList<T>> GetAllAsync(string? include = null);
    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);

    void Attache(T entity);

    Task<IReadOnlyList<T>> GetAllWithIdAsync(Expression<Func<T, bool>> predicate, string? include = null);

	Task<T?> FindByIdAsync(Expression<Func<T, bool>> predicate, string? include = null);

	Task<TResult?> FindAndProjectAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);

	void UpdateProperty<TProperty>(
	T entity,
	Expression<Func<T, TProperty>> propertyExpression);

}
