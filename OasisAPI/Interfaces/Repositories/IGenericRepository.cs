using System.Linq.Expressions;

namespace OasisAPI.Interfaces.Repositories;

public interface IGenericRepository<T>
{
    IQueryable<T> GetAll();
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
}