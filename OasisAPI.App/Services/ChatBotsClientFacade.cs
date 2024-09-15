using Domain.Entities;
using OasisAPI.App.Interfaces.Services;
using OasisAPI.Infra.Clients.Interfaces;
using OasisAPI.Infra.Dto;

namespace OasisAPI.App.Services;

public class ChatBotsClientFacade : IChatBotsClientFacade
{
    private readonly IDictionary<ChatBotEnum, IChatBotClient> _chatBotClients;

    public ChatBotsClientFacade(IEnumerable<IChatBotClient> chatBotClients)
    {
        _chatBotClients = chatBotClients.ToDictionary(client => client.ChatBotEnum);
    }
    
    public async Task<IEnumerable<ChatBotMessageDto>> StartConversationWithChatBots(string message, HashSet<ChatBotEnum> selectedChatBots)
    {
        var tasks = new List<Task<ChatBotMessageDto>>();
        
        foreach (var chatBotEnum in selectedChatBots)
        {
            var actualClient = _chatBotClients[chatBotEnum];
            
            switch (actualClient)
            {
                case ICreateThreadAndSendMessage c:
                    tasks.Add(c.CreateThreadAndSendMessageAsync(message));
                    break;
            }
        }
        
        return await ExecuteChatBotsTasks(tasks);
    }
    
    public Task<IEnumerable<ChatBotMessageDto>> ContinueConversationWithChatBotsAsync(string message, 
        IList<string> allMessages, HashSet<OasisChatBotDetails> chatBotDetailsSet)
    {
        var tasks = new List<Task<ChatBotMessageDto>>();

        foreach (var chatBotAndThreadDto in chatBotDetailsSet)
        {
            var actualClient = _chatBotClients[chatBotAndThreadDto.ChatBotEnum];
            
            switch (actualClient)
            {
                case ISendMessageToThread c:
                    tasks.Add(c.SendMessageToThreadAsync(chatBotAndThreadDto.ThreadId!, message));
                    break;
                
                case ISendAllMessagesToThread c:
                    tasks.Add(c.SendAllMessagesAsync(allMessages));
                    break;
            }
        }
        
        return ExecuteChatBotsTasks(tasks);
    }
    
    private async Task<IEnumerable<ChatBotMessageDto>> ExecuteChatBotsTasks(IEnumerable<Task<ChatBotMessageDto>> chatbotTasks)
    {
        var handlingTasks = chatbotTasks.Select(async task =>
        {
            try
            {
                return await task;
            }
            catch (Exception)
            {
                return new ChatBotMessageDto
                {
                    Message = "Internal error processing message",
                    ThreadId = "",
                    ChatBotEnum = task.Result.ChatBotEnum,
                    MessageId = ""
                };
            }
        });
        
        return await Task.WhenAll(handlingTasks);
    }


}