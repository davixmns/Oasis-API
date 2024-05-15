using OasisAPI.Context;
using OasisAPI.Interfaces;
using OasisAPI.Interfaces.Repositories;

namespace OasisAPI.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private IUserRepository userRepository;
    private IChatRepository chatRepository;
    private IMessageRepository messageRepository;

    public OasisDbContext context;

    public UnitOfWork(OasisDbContext context)
    {
        this.context = context;
    }

    public IUserRepository UserRepository => this.userRepository ?? new UserRepository(this.context);
    public IChatRepository ChatRepository => this.chatRepository ?? new ChatRepository(this.context);
    public IMessageRepository MessageRepository => this.messageRepository ?? new MessageRepository(this.context);
    
    public async Task CommitAsync()
    {
        await this.context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await this.context.DisposeAsync();
    }
}