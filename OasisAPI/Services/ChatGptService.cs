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

    public async Task<IActionResult> Test()
    {
        var assistantsList = await _api.AssistantsEndpoint.ListAssistantsAsync();
        
        return new JsonResult(assistantsList);
    }

    public async Task<IActionResult> CreateThread(string userMessage)
    {
        if(userMessage is "")
            return new BadRequestObjectResult("User message cannot be empty.");
        
        var assistant = await _api.AssistantsEndpoint.RetrieveAssistantAsync(_chatGptConfig.AssistantId);

        var threadRequest = new CreateThreadRequest(new List<Message>() { userMessage });

        var run = await assistant.CreateThreadAndRunAsync(threadRequest);

        while (true)
        {
            Console.WriteLine("Esperando resposta...");
            await Task.Delay(1000);
            ListResponse<MessageResponse> messages = await _api.ThreadsEndpoint.ListMessagesAsync(run.ThreadId);
            
            var lastMessage = await _api.ThreadsEndpoint.RetrieveMessageAsync(run.ThreadId, messages.FirstId);

            Console.WriteLine("Chatgpt: " + lastMessage.Content[0].Text.Value);
            
            if (lastMessage.Role == Role.Assistant)
                return new JsonResult(lastMessage);
        }
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