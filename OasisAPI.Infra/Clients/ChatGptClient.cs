using AutoMapper;
using Domain.Entities;
using OasisAPI.App.Config;
using OasisAPI.App.Utils;
using OpenAI;
using OpenAI.Threads;

namespace OasisAPI.Infra.Clients;

public class ChatGptClient : IChatGptClient
{
    private readonly OpenAIClient _chatGptApi;
    private readonly ChatGptConfig _chatGptConfig;
    private readonly IMapper _mapper;
    
    public ChatGptClient(ChatGptConfig chatGptConfig, IMapper mapper)
    {
        _chatGptApi = new OpenAIClient(chatGptConfig.ApiKey);
        _chatGptConfig = chatGptConfig;
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
        await _chatGptApi.ThreadsEndpoint.CreateMessageAsync(threadId, new Message(userMessage));
        
        var run = await _chatGptApi.ThreadsEndpoint.CreateRunAsync<RunResponse>(
            threadId: threadId,
            request: new CreateRunRequest(_chatGptConfig.AssistantId)
        );
        
        run = await run.WaitForStatusChangeAsync();
        
        var messageList = await run.ListMessagesAsync();

        return _mapper.Map<OasisMessage>(messageList.Items[0]);
    }
}