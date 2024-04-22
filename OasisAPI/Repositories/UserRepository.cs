using Microsoft.EntityFrameworkCore;
using OasisAPI.Context;
using OasisAPI.Interfaces;
using OasisAPI.Models;

namespace OasisAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly OasisDbContext _context;
    
    public UserRepository(OasisDbContext context)
    {
        _context = context;
    }
    
    public async Task<OasisUser> CreateUserAsync(OasisUser userData)
    {
        _context.OasisUsers.Add(userData);
        await _context.SaveChangesAsync();
        return userData;
    }
    
    public async Task<OasisUser?> GetUserByEmail(string email)
    {
        return await _context.OasisUsers.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<OasisUser?> GetUserById(int id)
    {
        return await _context.OasisUsers.FindAsync(id);
    }
}