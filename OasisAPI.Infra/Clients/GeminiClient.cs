using AutoMapper;
using Domain.Entities;
using Domain.Utils;
using GenerativeAI.Models;
using GenerativeAI.Types;
using OasisAPI.App.Config;
using OasisAPI.App.Utils;
using OasisAPI.Infra.Clients.Interfaces;
using OasisAPI.Infra.Dto;

namespace OasisAPI.Infra.Clients;

public class GeminiClient : IChatBotClient, ICreateThreadAndSendMessage, ISendAllMessagesToThread, IGetChatTitle
{
    public ChatBotEnum ChatBotEnum => ChatBotEnum.Gemini;
    
    private readonly GenerativeModel _geminiApi;
    private readonly IMapper _mapper;

    public GeminiClient(GeminiConfig geminiConfig, IMapper mapper)
    {
        _geminiApi = new GenerativeModel(
            apiKey: geminiConfig.ApiKey,
            model: geminiConfig.Model
        );
        _mapper = mapper;
    }

    public async Task<ChatBotMessageDto> CreateThreadAndSendMessageAsync(string message)
    {
        var chat = _geminiApi.StartChat(new StartChatParams());

        await chat.SendMessageAsync(PromptForChatbots.GeminiPromptText);

        var geminiResponse = await chat.SendMessageAsync(message);

        return _mapper.Map<ChatBotMessageDto>(geminiResponse);
    }

    public async Task<ChatBotMessageDto> SendAllMessagesAsync(IEnumerable<string> chatMessages)
    {
        var chat = _geminiApi.StartChat(new StartChatParams());

        await chat.SendMessageAsync(PromptForChatbots.GeminiPromptText);

        var formattedMessages = string.Join("\n\n", chatMessages);
        var geminiResponse = await chat.SendMessageAsync(formattedMessages);

        return _mapper.Map<ChatBotMessageDto>(geminiResponse);
    }

    public async Task<ChatBotMessageDto> GetChatTitleAsync(string userMessage)
    {
        var chat = _geminiApi.StartChat(new StartChatParams());

        var formattedMessage = OasisMessageFormatter.FormatToGetChatTitle(userMessage);

        var geminiResponse = await chat.SendMessageAsync(formattedMessage);

        return _mapper.Map<ChatBotMessageDto>(geminiResponse);
    }
}