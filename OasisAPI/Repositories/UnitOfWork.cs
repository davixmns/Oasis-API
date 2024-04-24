using OasisAPI.Context;
using OasisAPI.Interfaces;

namespace OasisAPI.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IUserRepository _userRepository;

    public OasisDbContext _context;

    public UnitOfWork(OasisDbContext context)
    {
        _context = context;
    }

    public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}