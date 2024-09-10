using Domain.Entities;

namespace OasisAPI.Infra.Repositories;

public interface IUnitOfWork
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
    Task CommitAsync();
    Task DisposeAsync();
}