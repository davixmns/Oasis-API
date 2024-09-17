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
        var newOasisChat = new OasisChat(
            oasisUserId: request.OasisUserId,
            title: request.Title
        );

        newOasisChat.ChatBots = new List<OasisChatBotDetails>()
        {
            new(newOasisChat.Id, ChatBotEnum.ChatGpt, true, null),
            new(newOasisChat.Id, ChatBotEnum.Gemini, true, null)
        };
        
        newOasisChat.Messages!.Add(new OasisMessage(
            oasisChatId: newOasisChat.Id,
            message: request.InitialMessage,
            from: ChatBotEnum.User
        ));
        
        _unitOfWork.GetRepository<OasisChat>().Create(newOasisChat);

        await _unitOfWork.CommitAsync();
        
        return AppResult<OasisChat>.Success(newOasisChat);
    }
}