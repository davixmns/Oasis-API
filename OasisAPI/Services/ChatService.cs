using Microsoft.EntityFrameworkCore;
using OasisAPI.Interfaces;
using OasisAPI.Interfaces.Services;
using OasisAPI.Models;

namespace OasisAPI.Services;

public sealed class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<OasisChat>> GetAllChatsAsync(int userId)
    {
        var chats = await _unitOfWork.ChatRepository
            .GetAll()
            .Where(chat => chat.OasisUserId == userId)
            .Include(chat => chat.Messages!.OrderByDescending(m => m.OasisMessageId))
            .ToListAsync();
        
        return chats;
    }
    
    public async Task<OasisChat> CreateChatAsync(OasisChat oasisChat)
    {
        
        var createdChat = _unitOfWork.ChatRepository.Create(oasisChat);
        await _unitOfWork.CommitAsync();
        return createdChat;
    }
    
    public async Task<OasisMessage> CreateMessageAsync(OasisMessage oasisMessage)
    {
        var createdMessage = _unitOfWork.MessageRepository.Create(oasisMessage);
        await _unitOfWork.CommitAsync();
        return createdMessage;
    }

    public async Task<OasisChat?> GetChatByIdAsync(int chatId)
    {
        return await _unitOfWork.ChatRepository.GetAsync(c => c.OasisChatId == chatId);
    }
    
    public async Task<IList<OasisMessage>> GetMessagesByChatId(int chatId)
    {
        return await _unitOfWork.MessageRepository
            .GetAll()
            .Where(message => message.OasisChatId == chatId)
            .ToListAsync();
    }
}