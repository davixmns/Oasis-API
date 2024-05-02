using OasisAPI.Context;
using OasisAPI.Interfaces.Repositories;
using OasisAPI.Models;

namespace OasisAPI.Repositories;

public class MessageRepository : GenericRepository<OasisMessage>, IMessageRepository
{
    public MessageRepository(OasisDbContext context) : base(context)
    {
    }
}