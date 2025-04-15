using Domain.Entities;
using MediatR;
using OasisAPI.App.Result;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Features.Chat.Queries.GetChatMessages;

public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, AppResult<OasisChat>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetChatMessagesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AppResult<OasisChat>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.GetRepository<OasisChat>()
            .GetAsync(c => c.Id == request.OasisChatId, c => c.Messages!, c => c.ChatBots);
        
        chat!.Messages = chat.Messages.Reverse().ToList();
        
        return AppResult<OasisChat>.Success(chat);
    }
}