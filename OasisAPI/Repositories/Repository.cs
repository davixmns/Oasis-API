using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OasisAPI.Context;
using OasisAPI.Interfaces;

namespace OasisAPI.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly OasisDbContext _context;
    
    public Repository(OasisDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<T> GetAll()
    {
        return _context.Set<T>();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        return entity;
    }
}