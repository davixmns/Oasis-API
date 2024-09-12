using Domain.Entities;
using MediatR;
using OasisAPI.Models;

namespace OasisAPI.App.Commands;

public class CreateChatCommand : IRequest<AppResult<OasisChat>>
{
    public int OasisUserId { get; }
    public string Title { get; }
    public string ChatGptThreadId { get; }
    public string GeminiThreadId { get; }
    
    public CreateChatCommand(int oasisUserId, string title, string chatGptThreadId, string geminiThreadId)
    {
        OasisUserId = oasisUserId;
        Title = title;
        ChatGptThreadId = chatGptThreadId;
        GeminiThreadId = geminiThreadId;
    }
}