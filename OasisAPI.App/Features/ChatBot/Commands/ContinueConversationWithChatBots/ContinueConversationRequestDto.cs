using Domain.Utils;

namespace OasisAPI.App.Features.ChatBot.Commands.ContinueConversationWithChatBots;

public class ContinueConversationRequestDto
{
    public int OasisChatId { get; init; }
    public string Message { get; init; } = string.Empty;
    public HashSet<ChatBotEnum> ChatBotEnums { get; init; } = [];
}