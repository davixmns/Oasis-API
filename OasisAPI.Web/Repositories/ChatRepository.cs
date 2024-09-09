using Domain.Entities;
using OasisAPI.Context;
using OasisAPI.Interfaces.Repositories;

namespace OasisAPI.Repositories;

public class ChatRepository : Repository<OasisChat>, IChatRepository
{
    public ChatRepository(OasisDbContext context) : base(context)
    {
    }
}