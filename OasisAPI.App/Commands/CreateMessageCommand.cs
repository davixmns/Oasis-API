using Domain.Entities;
using MediatR;
using OasisAPI.Models;

namespace OasisAPI.App.Commands;

public class CreateMessageCommand : IRequest<AppResult<OasisMessage>>
{
    public int OasisChatId { get; }
    public string Message { get; }
    public ChatBotEnum From { get; }
    public bool IsSaved { get; }
    
    public CreateMessageCommand(int oasisChatId, string message, ChatBotEnum from, bool isSaved)
    {
        OasisChatId = oasisChatId;
        Message = message;
        From = from;
        IsSaved = isSaved;
    }
}