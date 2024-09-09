using Domain.Entities;
using OasisAPI.Context;
using OasisAPI.Interfaces.Repositories;

namespace OasisAPI.Repositories;

public class UserRepository : Repository<OasisUser>, IUserRepository
{
    public UserRepository(OasisDbContext context) : base(context)
    {
    }
}