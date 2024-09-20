using Domain.Entities;
using MediatR;
using OasisAPI.App.Features.Chat.Commands.UpdateOasisChatDetails;
using OasisAPI.App.Result;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Features.Chat.Commands.UpdateOasisChatBotDetails;

public class UpdateOasisChatBotDetailsCommandHandler : IRequestHandler<UpdateOasisChatBotDetailsCommand, AppResult<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateOasisChatBotDetailsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AppResult<Unit>> Handle(UpdateOasisChatBotDetailsCommand request, CancellationToken cancellationToken)
    {
        var details = await _unitOfWork.GetRepository<OasisChatBotDetails>().GetAsync(c => c.Id == request.OasisChatBotDetailsId);
        
        if(details is null)
            return AppResult<Unit>.Success(Unit.Value);
        
        details.IsActive = request.IsActive;
        
        _unitOfWork.GetRepository<OasisChatBotDetails>().Update(details);
        
        await _unitOfWork.CommitAsync();
        
        return AppResult<Unit>.Success(Unit.Value);
    }
}