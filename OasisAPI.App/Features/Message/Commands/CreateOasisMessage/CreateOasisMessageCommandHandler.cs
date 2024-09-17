using Domain.Entities;
using MediatR;
using OasisAPI.App.Result;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Features.Message.Commands.CreateOasisMessage;

public class CreateOasisMessageCommandHandler : IRequestHandler<CreateOasisMessageCommand, AppResult<OasisMessage>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateOasisMessageCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AppResult<OasisMessage>> Handle(CreateOasisMessageCommand request, CancellationToken cancellationToken)
    {
        var newMessage = new OasisMessage(
            oasisChatId: request.OasisChatId,
            message: request.Message,
            from: request.From,
            isSaved: true
        );
        
        _unitOfWork.GetRepository<OasisMessage>().Create(newMessage);
        
        await _unitOfWork.CommitAsync();
        
        return AppResult<OasisMessage>.Success(newMessage);
    }
}