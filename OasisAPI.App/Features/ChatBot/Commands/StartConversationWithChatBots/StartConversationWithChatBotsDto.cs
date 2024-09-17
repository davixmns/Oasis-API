using Domain.Utils;

namespace OasisAPI.App.Features.ChatBot.Commands.StartConversationWithChatBots;

public class StartConversationWithChatBotsDto
{
    public string Message { get; init; } = string.Empty;
    public HashSet<ChatBotEnum> ChatBotEnums { get; init; } = [];
}