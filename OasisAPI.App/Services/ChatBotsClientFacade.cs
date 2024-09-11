using Domain.Entities;
using OasisAPI.App.Exceptions;
using OasisAPI.App.Interfaces.Services;
using OasisAPI.Enums;
using OasisAPI.Infra.Clients;

namespace OasisAPI.App.Services;

public class ChatBotsClientFacade : IChatBotsClientFacade
{
    private readonly IChatGptClient _chatGptClient;
    private readonly IGeminiClient _geminiClient;
    
    public ChatBotsClientFacade(IChatGptClient chatGptClient, IGeminiClient geminiClient)
    {
        _chatGptClient = chatGptClient;
        _geminiClient = geminiClient;
    }
    
    public async Task<IEnumerable<OasisMessage>> CreateThreadsAndSendMessageAsync(string message, HashSet<ChatBotsEnum> selectedChatBots)
    {
        var chatbotsTasks = new List<Task<OasisMessage>>
        {
            _geminiClient.GetChatTitleAsync(message) // Get title from Gemini
        };

        foreach (var chatBot in selectedChatBots)
        {
            switch (chatBot)
            {
                case ChatBotsEnum.ChatGpt:
                    chatbotsTasks.Add(_chatGptClient.CreateChatAndSendMessage(message));
                    break;
                case ChatBotsEnum.Gemini:
                    chatbotsTasks.Add(_geminiClient.CreateChatAndSendMessageAsync(message));
                    break;
                default:
                    throw new OasisException("Invalid chatbot");
            }
        }
        
        return await ExecuteChatBotsTasks(chatbotsTasks);
    }

    public Task<IEnumerable<OasisMessage>> SendMessageToThreadsAsync(string message, HashSet<ChatBotsEnum> chatBotsEnums)
    {
        throw new NotImplementedException();
    }
    
    private async Task<IEnumerable<OasisMessage>> ExecuteChatBotsTasks(IEnumerable<Task<OasisMessage>> chatbotTasks)
    {
        var completedTasks = await Task.WhenAll(chatbotTasks.Select(task => Task.Run(async () =>
        {
            try
            {
                return await task;
            }
            catch (Exception)
            {
                return new OasisMessage(
                    from: "Error",
                    message: "Error processing message, try again later",
                    isSaved: false
                );
            }
        })));

        return completedTasks.ToList();
    }
}