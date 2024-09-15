using MediatR;
using OasisAPI.App.Commands;
using OasisAPI.App.Interfaces.Services;
using OasisAPI.App.Result;
using OasisAPI.Infra.Dto;

namespace OasisAPI.App.Handlers;

public class StartConversationWithChatBotsCommandHandler : IRequestHandler<StartConversationWithChatBotsCommand, AppResult<IEnumerable<ChatBotMessageDto>>>
{
    private readonly IChatBotsClientFacade _chatBotsClientFacade;
    
    public StartConversationWithChatBotsCommandHandler(IChatBotsClientFacade chatBotsClientFacade)
    {
        _chatBotsClientFacade = chatBotsClientFacade;
    }
    
    public async Task<AppResult<IEnumerable<ChatBotMessageDto>>> Handle(StartConversationWithChatBotsCommand request, CancellationToken cancellationToken)
    {
        var receivedMessages = await _chatBotsClientFacade.StartConversationWithChatBots(request.Message, request.ChatBotsEnums);
        
        return AppResult<IEnumerable<ChatBotMessageDto>>.Success(receivedMessages);
    }
}