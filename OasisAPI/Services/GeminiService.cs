using GenerativeAI.Models;
using GenerativeAI.Services;
using GenerativeAI.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OasisAPI.Configurations;
using OasisAPI.Interfaces;
using OasisAPI.Models;

namespace OasisAPI.Services;

public class GeminiService : IGeminiService
{
    private readonly GeminiConfig _geminiConfig;
    private readonly GenerativeModel _api;

    public GeminiService(IOptions<GeminiConfig> geminiConfig)
    {
        _geminiConfig = geminiConfig.Value;
        _api = new GenerativeModel(
            apiKey: _geminiConfig.ApiKey
            
        );
    }

    public async Task<IActionResult> StartChat(string userMessage)
    {
        var chat = _api.StartChat(new StartChatParams());

        var result = await chat.SendMessageAsync(message: userMessage);
        
        return new JsonResult(new GeminiResponse(chat, result));
    }
}