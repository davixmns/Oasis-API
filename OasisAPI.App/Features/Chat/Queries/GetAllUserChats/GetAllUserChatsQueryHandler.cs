using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OasisAPI.App.Result;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Features.Chat.Queries.GetAllUserChats;

public class GetAllUserChatsQueryHandler : IRequestHandler<GetAllUserChatsQuery, AppResult<IEnumerable<OasisChat>>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllUserChatsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AppResult<IEnumerable<OasisChat>>> Handle(GetAllUserChatsQuery request, CancellationToken cancellationToken)
    {
        var chats = await _unitOfWork.GetRepository<OasisChat>()
            .GetAll()
            .Where(c => c.OasisUserId == request.UserId)
            // .Include(c => c.Messages)
            .Include(c => c.ChatBots)
            .ToListAsync(cancellationToken);
        
        return AppResult<IEnumerable<OasisChat>>.Success(chats);
    }
}