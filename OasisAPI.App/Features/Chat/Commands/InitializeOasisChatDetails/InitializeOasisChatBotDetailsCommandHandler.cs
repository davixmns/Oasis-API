using Domain.Entities;
using MediatR;
using OasisAPI.App.Features.Chat.Commands.UpdateOasisChatDetails;
using OasisAPI.App.Result;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Features.Chat.Commands.InitializeOasisChatDetails;

public class InitializeOasisChatBotDetailsCommandHandler : IRequestHandler<InitializeOasisChatBotDetailsCommand, AppResult<OasisChat>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public InitializeOasisChatBotDetailsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    //Update the chat title and chatbot thread ids
    public async Task<AppResult<OasisChat>> Handle(InitializeOasisChatBotDetailsCommand request, CancellationToken cancellationToken)
    {
        var chat = request.OasisChat;
        var chatBotMessages = request.Messages.ToList();
        
        chat.Title = chatBotMessages.First().Message;

        foreach (var c in chat.ChatBots)
        {
            var message = chatBotMessages.FirstOrDefault(m => m.ChatBotEnum == c.ChatBotEnum);
            c.ThreadId = message?.ThreadId;
        }
        
        _unitOfWork.GetRepository<OasisChat>().Update(chat);
        await _unitOfWork.CommitAsync();
        
        return AppResult<OasisChat>.Success(chat);
    }
}