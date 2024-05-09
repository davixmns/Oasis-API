using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OasisAPI.Context;
using OasisAPI.Interfaces;
using OasisAPI.Interfaces.Repositories;
using OasisAPI.Models;

namespace OasisAPI.Repositories;

public class UserRepository : GenericRepository<OasisUser>, IUserRepository
{
    public UserRepository(OasisDbContext context) : base(context)
    {
    }
}