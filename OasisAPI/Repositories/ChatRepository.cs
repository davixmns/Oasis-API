using OasisAPI.Context;
using OasisAPI.Interfaces.Repositories;
using OasisAPI.Models;

namespace OasisAPI.Repositories;

public class ChatRepository : GenericRepository<OasisChat>, IChatRepository
{
    public ChatRepository(OasisDbContext context) : base(context)
    {
    }
}