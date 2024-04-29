using OasisAPI.Models;

namespace OasisAPI.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    
    Task CommitAsync();
}