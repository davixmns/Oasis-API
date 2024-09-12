using AutoMapper;
using Domain.Entities;
using GenerativeAI.Models;
using GenerativeAI.Types;
using OasisAPI.App.Config;
using OasisAPI.App.Utils;
using OasisAPI.Infra.Dto;

namespace OasisAPI.Infra.Clients;

public class GeminiClient : IGeminiClient
{
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

    public async Task<ChatBotMessageResponseDto> CreateThreadAndSendMessageAsync(string message)
    {
        var chat = _geminiApi.StartChat(new StartChatParams());

        await chat.SendMessageAsync(PromptForChatbots.GeminiPromptText);

        var geminiResponse = await chat.SendMessageAsync(message);

        return _mapper.Map<ChatBotMessageResponseDto>(geminiResponse);
    }

    public async Task<ChatBotMessageResponseDto> SendMessageToThreadAsync(IEnumerable<string> chatMessages)
    {
        var chat = _geminiApi.StartChat(new StartChatParams());

        await chat.SendMessageAsync(PromptForChatbots.GeminiPromptText);

        var formattedMessages = string.Join("\n\n", chatMessages);
        var geminiResponse = await chat.SendMessageAsync(formattedMessages);

        return _mapper.Map<ChatBotMessageResponseDto>(geminiResponse);
    }

    public async Task<ChatBotMessageResponseDto> GetChatTitleAsync(string userMessage)
    {
        var chat = _geminiApi.StartChat(new StartChatParams());

        var formattedMessage = OasisMessageFormatter.FormatToGetChatTitle(userMessage);

        var geminiResponse = await chat.SendMessageAsync(formattedMessage);

        return _mapper.Map<ChatBotMessageResponseDto>(geminiResponse);
    }
}