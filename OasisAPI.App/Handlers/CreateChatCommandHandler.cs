using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using OasisAPI.App.Commands;
using OasisAPI.Infra.Repositories;
using OasisAPI.Models;

namespace OasisAPI.App.Handlers;

public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, AppResult<OasisChat>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateChatCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AppResult<OasisChat>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var newOasisChat = new OasisChat(
            oasisUserId: request.OasisUserId,
            title: request.Title
        );

        newOasisChat.ChatBots = new List<OasisChatBotInfo>()
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
        
        return AppResult<OasisChat>.SuccessResponse(newOasisChat);
    }
}