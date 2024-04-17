using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OasisAPI.Builders;
using OasisAPI.Configurations;
using OasisAPI.Interfaces;
using OasisAPI.Models;
using OpenAI;
using OpenAI.Threads;

namespace OasisAPI.Services;

public class ChatGptService : IChatGptService
{
    private readonly ChatGptConfig _chatGptConfig;
    private readonly OpenAIClient _api;

    public ChatGptService(IOptionsMonitor<ChatGptConfig> chatGptConfig)
    {
        _chatGptConfig = chatGptConfig.CurrentValue;
        _api = new OpenAIClient(_chatGptConfig.ApiKey);
    }
    
    public async Task<OasisMessage> StartChat(string userMessage)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
            throw new NullReferenceException("Mensagem do usuário vazia!");
        
        var thread = await _api.ThreadsEndpoint.CreateThreadAsync(userMessage);
        var run = await thread.CreateRunAsync(assistantId: _chatGptConfig.AssistantId);
        run = await run.WaitForStatusChangeAsync(); //Esperando a resposta do ChatGPT
        var messageList = await run.ListMessagesAsync();

        var builder = new OasisMessageBuilder()
            .SetFrom("ChatGPT")
            .SetMessage(messageList.Items[0].Content[0].Text.Value)
            .SetFromMessageId(messageList.Items[0].Id)
            .SetFromThreadId(run.ThreadId);

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