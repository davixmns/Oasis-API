using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Services.ChatService;

public sealed class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<OasisChat>> GetAllChatsAsync(int userId)
    {
        var chats = await _unitOfWork.GetRepository<OasisChat>()
            .GetAll()
            .Where(chat => chat.OasisUserId == userId)
            .Include(chat => chat.Messages!.OrderByDescending(m => m.Id))
            .ToListAsync();
        
        return chats;
    }
    
    public async Task<OasisChat> CreateChatAsync(OasisChat oasisChat)
    {
        var createdChat = _unitOfWork.GetRepository<OasisChat>().Create(oasisChat);
        await _unitOfWork.CommitAsync();
        return createdChat;
    }
    
    public async Task<OasisMessage> CreateMessageAsync(OasisMessage oasisMessage)
    {
        var createdMessage = _unitOfWork.GetRepository<OasisMessage>().Create(oasisMessage);
        await _unitOfWork.CommitAsync();
        return createdMessage;
    }

    public async Task<OasisChat?> GetChatByIdAsync(int chatId)
    {
        return await _unitOfWork.GetRepository<OasisChat>().GetAsync(c => c.Id == chatId);
    }
    
    public async Task<IList<OasisMessage>> GetMessagesByChatId(int chatId)
    {
        return await _unitOfWork.GetRepository<OasisMessage>()
            .GetAll()
            .Where(message => message.OasisChatId == chatId)
            .ToListAsync();
    }
}