using OasisAPI.Context;
using OasisAPI.Interfaces;
using OasisAPI.Interfaces.Repositories;

namespace OasisAPI.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IUserRepository _userRepository;
    private IChatRepository _chatRepository;

    public OasisDbContext context;

    public UnitOfWork(OasisDbContext context)
    {
        this.context = context;
    }

    public IUserRepository UserRepository => this._userRepository ?? new UserRepository(this.context);
    public IChatRepository ChatRepository => this._chatRepository ?? new ChatRepository(this.context);

    public async Task CommitAsync()
    {
        await this.context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await this.context.DisposeAsync();
    }
}