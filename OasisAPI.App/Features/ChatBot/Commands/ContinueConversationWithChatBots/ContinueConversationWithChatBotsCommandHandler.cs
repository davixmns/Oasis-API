using Domain.Entities;
using Domain.Utils;
using MediatR;
using OasisAPI.App.Result;
using OasisAPI.App.Services.ChatBotClientFacade;
using OasisAPI.Infra.Dto;
using OasisAPI.Infra.Repositories;
using OasisAPI.Infra.Utils;

namespace OasisAPI.App.Features.ChatBot.Commands.ContinueConversationWithChatBots;

public class ContinueConversationWithChatBotsCommandHandler : IRequestHandler<ContinueConversationWithChatBotsCommand,
    AppResult<IEnumerable<ChatBotMessageDto>>>
{
    private readonly IChatBotsClientFacade _chatBotsClientFacade;
    private readonly IUnitOfWork _unitOfWork;

    public ContinueConversationWithChatBotsCommandHandler(IChatBotsClientFacade chatBotsClientFacade,
        IUnitOfWork unitOfWork)
    {
        _chatBotsClientFacade = chatBotsClientFacade;
        _unitOfWork = unitOfWork;
    }

    //Debugar depois, não está conseguindo encontrar a ultima mensagem do chatbot
    public async Task<AppResult<IEnumerable<ChatBotMessageDto>>> Handle(ContinueConversationWithChatBotsCommand request,
        CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.GetRepository<OasisChat>().GetAsync(
            c => c.Id == request.OasisChatId,
            c => c.Messages!, c => c.ChatBots!
        );

        var selectedChatBots = chat!.ChatBots.Where(cb => cb.IsActive).ToHashSet();
        
        if(selectedChatBots.Count == 0)
            return AppResult<IEnumerable<ChatBotMessageDto>>.Fail("No chatbots selected");

        //Prepare all the messages to send to Gemini, Gemini dont save the past messages
        var allMessages = chat!.Messages!.Select(m => m).ToList();
        var allMessagesString = allMessages.Select(m => m.Message).ToList();

        //Choose the last message from a chatbot
        var latestChatBotMessage = allMessages.LastOrDefault(m => m.ChatBotEnum != ChatBotEnum.User)?.Message ?? "";
        var formattedMessageToGpt = OasisMessageFormatter.FormatToChatbotAndUserMessage(
            chatbotMessage: latestChatBotMessage,
            userMessage: request.Message
        );

        var receivedMessages = await _chatBotsClientFacade.ContinueConversationWithChatBotsAsync(
            message: formattedMessageToGpt,
            allMessages: allMessagesString,
            chatBotDetailsSet: selectedChatBots
        );

        return AppResult<IEnumerable<ChatBotMessageDto>>.Success(receivedMessages);
    }
}