using Domain.Entities;
using MediatR;
using OasisAPI.App.Commands;
using OasisAPI.Infra.Repositories;
using OasisAPI.Models;

namespace OasisAPI.App.Handlers;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, AppResult<OasisMessage>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateMessageCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AppResult<OasisMessage>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var newMessage = new OasisMessage(
            oasisChatId: request.OasisChatId,
            message: request.Message,
            from: request.From,
            isSaved: request.IsSaved
        );
        
        _unitOfWork.GetRepository<OasisMessage>().Create(newMessage);
        
        await _unitOfWork.CommitAsync();
        
        return AppResult<OasisMessage>.SuccessResponse(newMessage);
    }
}