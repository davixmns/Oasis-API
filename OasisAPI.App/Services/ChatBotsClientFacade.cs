using Domain.Entities;
using OasisAPI.App.Dto.Request;
using OasisAPI.App.Exceptions;
using OasisAPI.App.Interfaces.Services;
using OasisAPI.Infra.Clients;
using OasisAPI.Infra.Dto;

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
    
    //Create a new thread and send message to selected chatbots
    public async Task<IEnumerable<ChatBotMessageResponseDto>> CreateThreadsAndSendMessageAsync(string message, HashSet<ChatBotEnum> selectedChatBots)
    {
        var chatbotsTasks = new List<Task<ChatBotMessageResponseDto>>
        {
            _geminiClient.GetChatTitleAsync(message) // Get title from Gemini
        };

        foreach (var chatBot in selectedChatBots)
        {
            switch (chatBot)
            {
                case ChatBotEnum.ChatGpt:
                    chatbotsTasks.Add(_chatGptClient.CreateThreadAndSendMessageAsync(message));
                    break;
                case ChatBotEnum.Gemini:
                    chatbotsTasks.Add(_geminiClient.CreateThreadAndSendMessageAsync(message));
                    break;
                default:
                    throw new OasisException("Invalid chatbot");
            }
        }
        
        return await ExecuteChatBotsTasks(chatbotsTasks);
    }

    //Send message to a existing thread in selected chatbots
    public async Task<IEnumerable<ChatBotMessageResponseDto>> SendMessageToThreadsAsync(string message, IList<string> allMessages, HashSet<ChatBotAndThreadDto> chatBotAndThreadDtos)
    {
        var chatbotsTasks = new List<Task<ChatBotMessageResponseDto>>();
        
        foreach (var chatBot in chatBotAndThreadDtos)
        {
            switch (chatBot.ChatBotEnum)
            {
                case ChatBotEnum.ChatGpt:
                    chatbotsTasks.Add(_chatGptClient.SendMessageToThreadAsync(chatBot.threadId!, message));
                    break;
                case ChatBotEnum.Gemini:
                    chatbotsTasks.Add(_geminiClient.SendMessageToThreadAsync(allMessages));
                    break;
                default:
                    throw new OasisException("Invalid chatbot");
            }
        }
        
        return await ExecuteChatBotsTasks(chatbotsTasks);
    }
    
    private async Task<IEnumerable<ChatBotMessageResponseDto>> ExecuteChatBotsTasks(List<Task<ChatBotMessageResponseDto>> chatbotTasks)
    {
        var completedTasks = await Task.WhenAll(chatbotTasks.Select(task => Task.Run(async () =>
        {
            try
            {
                return await task;
            }
            catch (Exception)
            {
                return new ChatBotMessageResponseDto
                {
                    Message = "Internal Error in Chatbot, please try again later",
                    ThreadId = "",
                    ChatBotEnum = ChatBotEnum.Unknown,
                    MessageId = ""
                };
            }
        })));

        return completedTasks.ToList();
    }
}