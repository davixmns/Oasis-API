using AutoMapper;
using Domain.Utils;
using OasisAPI.App.Config;
using OasisAPI.App.Utils;
using OasisAPI.Infra.Clients.Interfaces;
using OasisAPI.Infra.Dto;
using OpenAI;
using OpenAI.Threads;

namespace OasisAPI.Infra.Clients;

public class ChatGptClient : IChatBotClient, ICreateThreadAndSendMessage, ISendMessageToThread
{
    public ChatBotEnum ChatBotEnum => ChatBotEnum.ChatGpt;
    
    private readonly OpenAIClient _chatGptApi;
    private readonly ChatGptConfig _chatGptConfig;
    private readonly IMapper _mapper;
    
    public ChatGptClient(ChatGptConfig chatGptConfig, IMapper mapper)
    {
        _chatGptApi = new OpenAIClient(chatGptConfig.ApiKey);
        _chatGptConfig = chatGptConfig;
        _mapper = mapper;
    }
    
    public async Task<ChatBotMessageDto> CreateThreadAndSendMessageAsync(string message)
    {
        var formattedMessage = OasisMessageFormatter.FormatToFirstUserMessage(message);
        
        var thread = await _chatGptApi.ThreadsEndpoint.CreateThreadAsync(formattedMessage);
        
        var run = await thread.CreateRunAsync(new CreateRunRequest(assistantId: _chatGptConfig.AssistantId));
        
        run = await run.WaitForStatusChangeAsync();
        
        var messageList = await run.ListMessagesAsync();
        
        var firstMessage = messageList.Items[0];

        return _mapper.Map<ChatBotMessageDto>(firstMessage);
    }

    public async Task<ChatBotMessageDto> SendMessageToThreadAsync(string threadId, string userMessage)
    {
        var thread = await _chatGptApi.ThreadsEndpoint.RetrieveThreadAsync(threadId);

        await thread.CreateMessageAsync(new Message(userMessage));
        
        var run = await thread.CreateRunAsync(new CreateRunRequest(_chatGptConfig.AssistantId));
        
        run = await run.WaitForStatusChangeAsync();
        
        var messageList = await run.ListMessagesAsync();

        return _mapper.Map<ChatBotMessageDto>(messageList.Items[0]);
    }

}