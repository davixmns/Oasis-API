using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Options;
using OasisAPI.Config;
using OasisAPI.Interfaces.Clients;

using OasisAPI.Utils;
using OpenAI;
using OpenAI.Threads;

namespace OasisAPI.Clients;

public class ChatGptClient : IChatGptClient
{
    private readonly OpenAIClient _chatGptApi;
    private readonly ChatGptConfig _chatGptConfig;
    private readonly IMapper _mapper;
    
    public ChatGptClient(IOptions<ChatGptConfig> chatGptConfig, IMapper mapper)
    {
        _chatGptApi = new OpenAIClient(chatGptConfig.Value.ApiKey);
        _chatGptConfig = chatGptConfig.Value;
        _mapper = mapper;
    }
    
    public async Task<OasisMessage> CreateChatAndSendMessage(string userMessage)
    {
        var formatedUserMessage = OasisMessageFormatter.FormatToFirstUserMessage(userMessage);
        
        var thread = await _chatGptApi.ThreadsEndpoint.CreateThreadAsync(formatedUserMessage);
        
        var run = await thread.CreateRunAsync(new CreateRunRequest(assistantId: _chatGptConfig.AssistantId));
        run = await run.WaitForStatusChangeAsync();
        
        var messageList = await run.ListMessagesAsync();

        return _mapper.Map<OasisMessage>(messageList.Items[0]);
    }

    public async Task<OasisMessage> SendMessageToChat(string threadId, string userMessage)
    {
        var messageRequest = new CreateMessageRequest(userMessage);
        await _chatGptApi.ThreadsEndpoint.CreateMessageAsync(threadId, messageRequest);
        
        var runRequest = new CreateRunRequest(_chatGptConfig.AssistantId);
        var run = await _chatGptApi.ThreadsEndpoint.CreateRunAsync(threadId, runRequest);
        
        run = await run.WaitForStatusChangeAsync();
        var messageList = await run.ListMessagesAsync();

        return _mapper.Map<OasisMessage>(messageList.Items[0]);
    }
}