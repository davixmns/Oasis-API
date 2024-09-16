using Domain.Entities;
using Domain.Utils;

namespace OasisAPI.App.Dto.Request;

public class CreateOasisMessageRequestDto
{
    public int OasisChatId { get; set; }
    public string Message { get; set; } = string.Empty;
    public ChatBotEnum ChatBotEnum { get; set; }
}