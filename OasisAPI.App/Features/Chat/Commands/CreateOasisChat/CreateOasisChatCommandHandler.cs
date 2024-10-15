using Domain.Entities;
using Domain.Utils;
using MediatR;
using OasisAPI.App.Result;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Features.Chat.Commands.CreateOasisChat;

public class CreateOasisChatCommandHandler : IRequestHandler<CreateOasisChatCommand, AppResult<OasisChat>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateOasisChatCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AppResult<OasisChat>> Handle(CreateOasisChatCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.GetRepository<OasisUser>().GetAsync(u => u.Id == request.OasisUserId) ?? throw new Exception("User not found");
        
        var createdChat = user.AddChat(request.Title);
        
        createdChat.AddMessage(ChatBotEnum.User, request.InitialMessage);

        await _unitOfWork.CommitAsync();
        
        return AppResult<OasisChat>.Success(createdChat);
    }
}