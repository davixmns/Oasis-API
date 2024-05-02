using AutoMapper;
using GenerativeAI.Models;
using GenerativeAI.Types;
using Microsoft.Extensions.Options;
using OasisAPI.Config;
using OasisAPI.Interfaces;
using OasisAPI.Interfaces.Services;
using OasisAPI.Models;
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
    
    public async Task<OasisMessage> StartGptChat(string userMessage)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
            throw new NullReferenceException("Mensagem do usuário vazia!");
        
        var thread = await _chatGptApi.ThreadsEndpoint.CreateThreadAsync(userMessage);
        var run = await thread.CreateRunAsync(assistantId: _chatGptConfig.AssistantId);
        run = await run.WaitForStatusChangeAsync(); //Esperando a resposta do ChatGPT
        var messageList = await run.ListMessagesAsync();

       return _mapper.Map<OasisMessage>(messageList.Items[0]);
    }
    
    public async Task<OasisMessage> StartGeminiChat(string userMessage)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
            throw new NullReferenceException("Mensagem do usuário vazia!");
        
        var chat = _geminiApi.StartChat(new StartChatParams());
        var geminiResponse = await chat.SendMessageAsync(userMessage);

        return _mapper.Map<OasisMessage>(geminiResponse);
    }

    public Task<OasisMessage> SendMessageGpt(string userMessage, string threadId)
    {
        throw new NotImplementedException();
    }

    public Task<OasisMessage> SendMessageGemini(string userMessage, string threadId)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<OasisMessage>> RetrieveGeminiMessageList(string threadId)
    {
        throw new NotImplementedException();
    }

    public Task<OasisMessage> RetrieveGptMessage(string messageId, string threadId)
    {
        throw new NotImplementedException();
    }
}