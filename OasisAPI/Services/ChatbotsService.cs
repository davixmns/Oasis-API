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
        if (string.IsNullOrWhiteSpace(userMessage))
        {
            throw new NullReferenceException("User message is empty!");
        }

        var thread = await _chatGptApi.ThreadsEndpoint.CreateThreadAsync(userMessage).ConfigureAwait(false);
        var run = await thread.CreateRunAsync(assistantId: _chatGptConfig.AssistantId).ConfigureAwait(false);
        run = await run.WaitForStatusChangeAsync().ConfigureAwait(false);
        var messageList = await run.ListMessagesAsync().ConfigureAwait(false);

        return _mapper.Map<OasisMessage>(messageList.Items[0]);
    }

    public async Task<OasisMessage> CreateGeminiChat(string userMessage)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
        {
            throw new NullReferenceException("User message is empty!");
        }

        var chat = _geminiApi.StartChat(new StartChatParams());
        await chat.SendMessageAsync(PromptForChatbots.PromptText).ConfigureAwait(false);
        var geminiResponse = await chat.SendMessageAsync(userMessage).ConfigureAwait(false);

        return _mapper.Map<OasisMessage>(geminiResponse);
    }

    public async Task<OasisMessage> SendMessageToGptChat(string threadId, string userMessage)
    {
        if (string.IsNullOrWhiteSpace(userMessage) || string.IsNullOrWhiteSpace(threadId))
        {
            throw new NullReferenceException("there are invalid parameters");
        }

        var messageRequest = new CreateMessageRequest(userMessage);
        await _chatGptApi.ThreadsEndpoint.CreateMessageAsync(threadId, messageRequest).ConfigureAwait(false);
        
        var runRequest = new CreateRunRequest(_chatGptConfig.AssistantId);
        var run = await _chatGptApi.ThreadsEndpoint.CreateRunAsync(threadId, runRequest).ConfigureAwait(false);
        
        run = await run.WaitForStatusChangeAsync().ConfigureAwait(false);
        var messageList = await run.ListMessagesAsync().ConfigureAwait(false);

        return _mapper.Map<OasisMessage>(messageList.Items[0]);
    }
    
    public async Task<OasisMessage> SendMessageToGeminiChat(IEnumerable<OasisMessage> chatMessages)
    {
        var chat = _geminiApi.StartChat(new StartChatParams());
       
        await chat
            .SendMessageAsync(PromptForChatbots.PromptText)
            .ConfigureAwait(false);
        
        var geminiResponse = await chat
            .SendMessageAsync(string.Join("\n", chatMessages.Select(m => m.Message)))
            .ConfigureAwait(false);
        
        return _mapper.Map<OasisMessage>(geminiResponse);
    }

    public async Task<OasisMessage> RetrieveChatTheme(string userMessage)
    {
        var formattedMessage = "Em até 3 palavras curtas diga o tema desta mensagem: " + userMessage;
        var messageTheme = await _geminiApi.GenerateContentAsync(formattedMessage);
        return _mapper.Map<OasisMessage>(messageTheme);
    }
}