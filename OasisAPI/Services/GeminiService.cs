using GenerativeAI.Methods;
using GenerativeAI.Models;
using GenerativeAI.Services;
using GenerativeAI.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OasisAPI.Builders;
using OasisAPI.Configurations;
using OasisAPI.Interfaces;
using OasisAPI.Models;

namespace OasisAPI.Services;

public class GeminiService : IChatbotService
{
    private readonly GeminiConfig _geminiConfig;
    private readonly GenerativeModel _api;

    public GeminiService(IOptions<GeminiConfig> geminiConfig)
    {
        _geminiConfig = geminiConfig.Value;
        _api = new GenerativeModel(apiKey: _geminiConfig.ApiKey);
    }

    public async Task<OasisMessage> StartChat(string userMessage)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
            throw new NullReferenceException("Mensagem do usuário vazia!");
        
        var chat = _api.StartChat(new StartChatParams());
        var geminiResponse = await chat.SendMessageAsync(userMessage);

        var builder = new OasisMessageBuilder()
            .SetName("Gemini")
            .SetMessage(geminiResponse);
        
        return builder.Build();
    }

    public Task<OasisMessage> SendMessage(string userMessage, string threadId)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<OasisMessage>> RetrieveMessageList(string threadId)
    {
        throw new NotImplementedException();
    }

    public Task<OasisMessage> RetrieveMessage(string messageId, string threadId)
    {
        throw new NotImplementedException();
    }
}