using Domain.Entities;

namespace OasisAPI.App.Services.ChatService;

public interface IChatService
{
    Task<OasisChat> CreateChatAsync(OasisChat oasisChat);
    Task<IEnumerable<OasisChat>> GetAllChatsAsync(int userId);
    Task<OasisChat?> GetChatByIdAsync(int chatId);
    Task<OasisMessage> CreateMessageAsync(OasisMessage oasisMessage);
    Task<IList<OasisMessage>> GetMessagesByChatId(int chatId);
}