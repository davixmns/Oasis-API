using OasisAPI.Interfaces.Repositories;

namespace OasisAPI.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IChatRepository ChatRepository { get; }
    IMessageRepository MessageRepository { get; }
    
    Task CommitAsync();
}