using Domain.Entities;
using MediatR;
using OasisAPI.App.Commands;
using OasisAPI.App.Dto.Request;
using OasisAPI.App.Interfaces.Services;
using OasisAPI.Infra.Dto;
using OasisAPI.Infra.Repositories;
using OasisAPI.Models;

namespace OasisAPI.App.Handlers;

public class ContinueConversationWithChatBotsCommandHandler : IRequestHandler<ContinueConversationWithChatBotsCommand, AppResult<IEnumerable<ChatBotMessageResponseDto>>>
{
    private readonly IChatBotsClientFacade _chatBotsClientFacade;
    private readonly IUnitOfWork _unitOfWork;
    
    public ContinueConversationWithChatBotsCommandHandler(IChatBotsClientFacade chatBotsClientFacade, IUnitOfWork unitOfWork)
    {
        _chatBotsClientFacade = chatBotsClientFacade;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AppResult<IEnumerable<ChatBotMessageResponseDto>>> Handle(ContinueConversationWithChatBotsCommand request, CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.GetRepository<OasisChat>().GetAsync(
            c => c.Id == request.OasisChatId,
            oasisChat => oasisChat.Messages!, oasisChat => oasisChat.ChatBots! 
        );
        
        //Prepare all the messages to send to Gemini, Gemini dont save the past messages
        var allMessages = chat!.Messages!.Select(m => m.Message).ToList();
        allMessages.Add(request.Message);

        var chatBotAndThreadDtos = chat.ChatBots.Select(c => new ChatBotAndThreadDto(
            chatBotEnum: c.ChatBotEnum,
            threadId: c.ThreadId!
        )).ToHashSet();
        
        var receivedMessages = await _chatBotsClientFacade.SendMessageToThreadsAsync(request.Message, allMessages, chatBotAndThreadDtos);
        
        return AppResult<IEnumerable<ChatBotMessageResponseDto>>.SuccessResponse(receivedMessages);
    }
}