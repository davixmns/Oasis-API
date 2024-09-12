using Domain.Entities;
using MediatR;
using OasisAPI.App.Commands;
using OasisAPI.App.Interfaces.Services;
using OasisAPI.Infra.Dto;
using OasisAPI.Models;

namespace OasisAPI.App.Handlers;

public class StartConversationWithChatBotsCommandHandler : IRequestHandler<StartConversationWithChatBotsCommand, AppResult<IEnumerable<ChatBotMessageResponseDto>>>
{
    private readonly IChatBotsClientFacade _chatBotsClientFacade;
    
    public StartConversationWithChatBotsCommandHandler(IChatBotsClientFacade chatBotsClientFacade)
    {
        _chatBotsClientFacade = chatBotsClientFacade;
    }
    
    public async Task<AppResult<IEnumerable<ChatBotMessageResponseDto>>> Handle(StartConversationWithChatBotsCommand request, CancellationToken cancellationToken)
    {
        var receivedMessages = await _chatBotsClientFacade.CreateThreadsAndSendMessageAsync(request.Message, request.ChatBotsEnums);
        
        return AppResult<IEnumerable<ChatBotMessageResponseDto>>.SuccessResponse(receivedMessages);
    }
}