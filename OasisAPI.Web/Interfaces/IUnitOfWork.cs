using Domain.Entities;
using OasisAPI.Interfaces.Repositories;

namespace OasisAPI.Interfaces;

public interface IUnitOfWork
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
    Task CommitAsync();
    Task DisposeAsync();
}