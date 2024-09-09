using Domain.Entities;
using OasisAPI.Context;
using OasisAPI.Interfaces.Repositories;

namespace OasisAPI.Repositories;

public class MessageRepository : Repository<OasisMessage>, IMessageRepository
{
    public MessageRepository(OasisDbContext context) : base(context)
    {
    }
}