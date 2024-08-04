using OasisAPI.Models;

namespace OasisAPI.Interfaces.Services;

public interface IChatService
{
    Task<OasisChat> CreateChatAsync(OasisChat oasisChat);
    Task<IEnumerable<OasisChat>> GetAllChatsAsync(int userId);
    Task<OasisChat?> GetChatByIdAsync(int chatId);
    Task<OasisMessage> CreateMessageAsync(OasisMessage oasisMessage);
    Task<IList<OasisMessage>> GetMessagesByChatId(int chatId);
}