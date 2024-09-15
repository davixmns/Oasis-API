using Domain.Entities;
using MediatR;
using OasisAPI.App.Commands;
using OasisAPI.App.Dto.Request;
using OasisAPI.App.Interfaces.Services;
using OasisAPI.App.Result;
using OasisAPI.App.Utils;
using OasisAPI.Infra.Dto;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Handlers;

public class ContinueConversationWithChatBotsCommandHandler : IRequestHandler<ContinueConversationWithChatBotsCommand, AppResult<IEnumerable<ChatBotMessageDto>>>
{
    private readonly IChatBotsClientFacade _chatBotsClientFacade;
    private readonly IUnitOfWork _unitOfWork;
    
    public ContinueConversationWithChatBotsCommandHandler(IChatBotsClientFacade chatBotsClientFacade, IUnitOfWork unitOfWork)
    {
        _chatBotsClientFacade = chatBotsClientFacade;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AppResult<IEnumerable<ChatBotMessageDto>>> Handle(ContinueConversationWithChatBotsCommand request, CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.GetRepository<OasisChat>().GetAsync(c => c.Id == request.OasisChatId);
        
        //Prepare all the messages to send to Gemini, Gemini dont save the past messages
        var allMessages = chat!.Messages!.Select(m => m).ToList();
        var allMessagesString = allMessages.Select(m => m.Message).ToList();
        //Include the new message on string list
        allMessagesString.Add(request.Message); 

        //Choose the last message from a chatbot
        var latestChatBotMessage = allMessages.LastOrDefault(m => m.From != ChatBotEnum.User)!.Message;
        var formattedMessageToGpt = OasisMessageFormatter.FormatToChatbotAndUserMessage(
            chatbotMessage:latestChatBotMessage,
            userMessage: request.Message
        );

        var receivedMessages = await _chatBotsClientFacade.ContinueConversationWithChatBotsAsync(
            message: formattedMessageToGpt,
            allMessages: allMessagesString,
            chatBotDetailsSet: chat.ChatBots.ToHashSet()
        );
        
        return AppResult<IEnumerable<ChatBotMessageDto>>.Success(receivedMessages);
    }
}