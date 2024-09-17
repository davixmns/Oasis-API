using Domain.Utils;

namespace OasisAPI.App.Features.Message.Commands.CreateOasisMessage;

public class CreateOasisMessageRequestDto
{
    public int OasisChatId { get; set; }
    public string Message { get; set; } = string.Empty;
    public ChatBotEnum ChatBotEnum { get; set; }
}