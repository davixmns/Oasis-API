using AutoMapper;
using GenerativeAI.Models;
using GenerativeAI.Types;
using Microsoft.Extensions.Options;
using OasisAPI.Config;
using OasisAPI.Interfaces.Clients;
using OasisAPI.Models;
using OasisAPI.Utils;

namespace OasisAPI.Clients;

public class GeminiClient : IGeminiClient
{
    private readonly GenerativeModel _geminiApi;
    private readonly IMapper _mapper;
    
    public GeminiClient(IOptions<GeminiConfig> geminiConfig, IMapper mapper)
    {
        _geminiApi = new GenerativeModel(geminiConfig.Value.ApiKey);
        _mapper = mapper;
    }
    
    public async Task<OasisMessage> CreateChatAndSendMessageAsync(string userMessage)
    {
        var chat = _geminiApi.StartChat(new StartChatParams());
        
        await chat.SendMessageAsync(PromptForChatbots.GeminiPromptText);
        
        var geminiResponse = await chat.SendMessageAsync(userMessage);
        
        return _mapper.Map<OasisMessage>(geminiResponse);
    }
    
    public async Task<OasisMessage> SendMessageToChatAsync(IEnumerable<OasisMessage> chatMessages)
    {
        var chat = _geminiApi.StartChat(new StartChatParams());
        
        await chat.SendMessageAsync(PromptForChatbots.GeminiPromptText);
        
        var formattedMessages = string.Join("\n\n", chatMessages.Select(m => m.Message));
        var geminiResponse = await chat.SendMessageAsync(formattedMessages);
        
        return _mapper.Map<OasisMessage>(geminiResponse);
    }

    public async Task<OasisMessage> GetChatTitleAsync(string userMessage)
    {
        var chat = _geminiApi.StartChat(new StartChatParams());
        
        var formattedMessage = OasisMessageFormatter.FormatToGetChatTitle(userMessage);
        
        var geminiResponse = await chat.SendMessageAsync(formattedMessage);
        
        return _mapper.Map<OasisMessage>(geminiResponse);
    }
}