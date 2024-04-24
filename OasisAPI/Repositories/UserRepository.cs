using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OasisAPI.Context;
using OasisAPI.Interfaces;
using OasisAPI.Models;

namespace OasisAPI.Repositories;

public class UserRepository : Repository<OasisUser>, IUserRepository
{
    public UserRepository(OasisDbContext context) : base(context)
    {
    }

    public async Task<OasisUser?> GetUserByEmailAsync(string email)
    {
        return await GetAll()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();
    }
}