using Domain.Entities;
using OasisAPI.Infra.Context;

namespace OasisAPI.Infra.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly OasisDbContext _dbContext;
    private readonly Dictionary<Type, object> _repositories;
    
    public UnitOfWork(OasisDbContext dbContext)
    {
        _dbContext = dbContext;
        _repositories = new Dictionary<Type, object>();
    }
    
    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
    {
        var type = typeof(TEntity);

        if (!_repositories.ContainsKey(type))
        {
            var repositoryInstance = new Repository<TEntity>(_dbContext);
            _repositories[type] = repositoryInstance!;
        }
        
        return (IRepository<TEntity>)_repositories[type];
    }
    
    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}