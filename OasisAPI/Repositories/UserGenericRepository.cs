using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OasisAPI.Context;
using OasisAPI.Interfaces;
using OasisAPI.Models;

namespace OasisAPI.Repositories;

public class UserGenericRepository : GenericRepository<OasisUser>, IUserRepository
{
    public UserGenericRepository(OasisDbContext context) : base(context)
    {
    }

    public async Task<OasisUser?> GetUserByEmailAsync(string email)
    {
        return await GetAll()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();
    }
}