using AutoMapper;
using GenerativeAI.Models;
using GenerativeAI.Types;
using Microsoft.Extensions.Options;
using OasisAPI.Config;
using OasisAPI.Interfaces.Services;
using OasisAPI.Models;
using OasisAPI.Utils;
using OpenAI;
using OpenAI.Threads;

namespace OasisAPI.Services;

public class ChatbotsService : IChatbotsService
{
    private readonly GenerativeModel _geminiApi;
    private readonly OpenAIClient _chatGptApi;
    private readonly ChatGptConfig _chatGptConfig;
    private readonly IMapper _mapper;

    public ChatbotsService(IOptions<GeminiConfig> geminiConfig, IOptions<ChatGptConfig> chatGptConfig, IMapper mapper)
    {
        _geminiApi = new GenerativeModel(geminiConfig.Value.ApiKey);
        _chatGptApi = new OpenAIClient(chatGptConfig.Value.ApiKey);
        _chatGptConfig = chatGptConfig.Value;
        _mapper = mapper;
    }

    public async Task<OasisMessage> CreateGptChat(string userMessage)
    {
        var thread = await _chatGptApi.ThreadsEndpoint.CreateThreadAsync(userMessage);
        var run = await thread.CreateRunAsync(assistantId: _chatGptConfig.AssistantId);
        run = await run.WaitForStatusChangeAsync();
        var messageList = await run.ListMessagesAsync();

        return _mapper.Map<OasisMessage>(messageList.Items[0]);
    }

    public async Task<OasisMessage> CreateGeminiChat(string userMessage)
    {
        var chat = _geminiApi.StartChat(new StartChatParams());
        await chat.SendMessageAsync(PromptForChatbots.GeminiPromptText);
        var geminiResponse = await chat.SendMessageAsync(userMessage);

        return _mapper.Map<OasisMessage>(geminiResponse);
    }

    public async Task<OasisMessage> SendMessageToGptChat(string threadId, string userMessage)
    {
        var messageRequest = new CreateMessageRequest(userMessage);
        await _chatGptApi.ThreadsEndpoint.CreateMessageAsync(threadId, messageRequest);
        
        var runRequest = new CreateRunRequest(_chatGptConfig.AssistantId);
        var run = await _chatGptApi.ThreadsEndpoint.CreateRunAsync(threadId, runRequest);
        
        run = await run.WaitForStatusChangeAsync();
        var messageList = await run.ListMessagesAsync();

        return _mapper.Map<OasisMessage>(messageList.Items[0]);
    }
    
    public async Task<OasisMessage> SendMessageToGeminiChat(IEnumerable<OasisMessage> chatMessages)
    {
        var chat = _geminiApi.StartChat(new StartChatParams());

        await chat.SendMessageAsync(PromptForChatbots.GeminiPromptText);

        var geminiResponse = await chat.SendMessageAsync(string.Join("\n", chatMessages.Select(m => m.Message)));
        
        return _mapper.Map<OasisMessage>(geminiResponse);
    }

    public async Task<OasisMessage> RetrieveChatTheme(string userMessage)
    {
        var formattedMessage = "Preciso que voce me diga como seria o titulo dessa mensagem, use no minimo 1 palavra e no maximo 3, precisa ser um titulo curto: " + userMessage;
        var messageTheme = await _geminiApi.GenerateContentAsync(formattedMessage);
        return _mapper.Map<OasisMessage>(messageTheme);
    }
}