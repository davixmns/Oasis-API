using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OasisAPI.Context;
using OasisAPI.Interfaces.Repositories;

namespace OasisAPI.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly OasisDbContext Database;

    public Repository(OasisDbContext database)
    {
        Database = database;
    }
    
    public IQueryable<TEntity> GetAll()
    {
        return Database.Set<TEntity>().AsQueryable();
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = Database.Set<TEntity>();

        // Aplicar includes se houver
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(predicate);
    }

    public TEntity Create(TEntity entity)
    {
        Database.Set<TEntity>().Add(entity);
        return entity;
    }

    public TEntity Update(TEntity entity)
    {
        Database.Set<TEntity>().Update(entity);
        return entity;
    }

    public TEntity Delete(TEntity entity)
    {
        Database.Set<TEntity>().Remove(entity);
        
        return entity;
    }
}