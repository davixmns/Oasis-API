using Domain.Entities;
using MediatR;
using OasisAPI.App.Result;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Features.Chat.Queries.GetChatMessages;

public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, AppResult<IEnumerable<OasisMessage>>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetChatMessagesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AppResult<IEnumerable<OasisMessage>>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.GetRepository<OasisChat>()
            .GetAsync(c => c.Id == request.OasisChatId, c => c.Messages!);

        return AppResult<IEnumerable<OasisMessage>> 
            .Success(chat?.Messages ?? []);
    }
}