using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OasisAPI.Configurations;
using OasisAPI.Models;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Chat;
using OpenAI.Threads;
using Message = OpenAI.Threads.Message;

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
    
    public async Task<IActionResult> CreateThreadSendMessageAndRun(string userMessage)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
            return new BadRequestObjectResult("A mensagem não pode ser vazia.");
        var threadRequest = new CreateThreadRequest(new List<Message>() { userMessage });
        var thread = await _api.ThreadsEndpoint.CreateThreadAsync(threadRequest);
        var run = await thread.CreateRunAsync(assistantId: _chatGptConfig.AssistantId);
        run = await run.WaitForStatusChangeAsync(); //Esperando a resposta do ChatGPT
        var messageList = await run.ListMessagesAsync();
        return new JsonResult(messageList);
    }

    public async Task<IActionResult> SendMessageToThread(string threadId, string userMessage)
    {
        CreateMessageRequest messageRequest = new(userMessage);
        var response = await _api.ThreadsEndpoint.CreateMessageAsync(threadId, messageRequest);
        return new JsonResult(response);
    }

    public async Task<IActionResult> RetrieveMessageList(string threadId)
    {
        var messages = await _api.ThreadsEndpoint.ListMessagesAsync(threadId);
        return new JsonResult(messages);
    }

    public async Task<IActionResult> RetrieveMessage(string threadId, string messageId)
    {
        var message = await _api.ThreadsEndpoint.RetrieveMessageAsync(threadId, messageId);

        return new JsonResult(message);
    }
}