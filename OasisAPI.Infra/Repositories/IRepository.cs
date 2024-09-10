using System.Linq.Expressions;
using Domain.Entities;

namespace OasisAPI.Infra.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, 
        params Expression<Func<TEntity, object>>[] includes);
    TEntity Create(TEntity entity);
    TEntity Update(TEntity entity);
    TEntity Delete(TEntity entity);
}