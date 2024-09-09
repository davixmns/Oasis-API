using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OasisAPI.Dto;
using OasisAPI.Enums;
using OasisAPI.Exceptions;
using OasisAPI.Interfaces.Clients;
using OasisAPI.Interfaces.Services;
using OasisAPI.Models;
using OasisAPI.Utils;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public sealed class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IChatGptClient _chatGptClient;
    private readonly IGeminiClient _geminiClient;

    public ChatController(IChatService chatService, IChatGptClient chatGptClient, IGeminiClient geminiClient)
    {
        _chatService = chatService;
        _chatGptClient = chatGptClient;
        _geminiClient = geminiClient;
    }

    [Authorize]
    [HttpGet("GetAllChats")]
    public async Task<IActionResult> GetAllChats()
    {
        var userId = GetUserIdFromContext();
        
        var chatsResponse = await _chatService.GetAllChatsAsync(userId);
        
        return Ok(OasisApiResult<IEnumerable<OasisChat>>.SuccessResponse(chatsResponse));
    }

    [Authorize]
    [HttpPost("SendFirstMessage")]
    public async Task<IActionResult> SendFirstMessage([FromBody] MessageRequestDto messageRequestDto)
    {
        var userId = GetUserIdFromContext();
        
        var chatbotsMessages = await CreateThreadAndSendMessageToChatbots(messageRequestDto);

        var createdChat = await _chatService.CreateChatAsync(new OasisChat(
            oasisUserId: userId,
            title: chatbotsMessages.FirstOrDefault()?.Message ?? "Untitled",
            chatGptThreadId: chatbotsMessages.FirstOrDefault(m => m.From == "ChatGPT")?.FromThreadId,
            geminiThreadId: chatbotsMessages.FirstOrDefault(m => m.From == "Gemini")?.FromThreadId
        ));

        await _chatService.CreateMessageAsync(new OasisMessage(
            from: "User",
            message: messageRequestDto.Message,
            oasisChatId: createdChat.Id,
            isSaved: true
        ));

        var response = new
        {
            chat = createdChat,
            chatbotMessages = chatbotsMessages.Skip(1)
        };

        return StatusCode(StatusCodes.Status201Created, OasisApiResult<object>.SuccessResponse(response));
    }

    [Authorize]
    [HttpPost("SendMessage/{oasisChatId:int}")]
    public async Task<IActionResult> SendMessageToChat(int oasisChatId, [FromBody] MessageRequestDto messageRequestDto)
    {
        var chat = await _chatService.GetChatByIdAsync(oasisChatId);
        
        if (chat is null) 
            return NotFound(OasisApiResult<string>.ErrorResponse("Chat not found"));

        await _chatService.CreateMessageAsync(new OasisMessage(
            from: "User",
            message: messageRequestDto.Message,
            oasisChatId: oasisChatId,
            isSaved: true
        ));

        var chatMessages = await _chatService.GetMessagesByChatId(oasisChatId);
        var latestChatbotMessage = chatMessages.LastOrDefault(m => m.From != "User")?.Message;

        var formattedMessageToGpt = OasisMessageFormatter.FormatToChatbotAndUserMessage(latestChatbotMessage!, messageRequestDto.Message);
        
        var chatbotMessages = await SendMessageToChatbotsThreads(messageRequestDto.ChatBotEnums, chat, formattedMessageToGpt);

        return Ok(OasisApiResult<List<OasisMessage>>.SuccessResponse(chatbotMessages));
    }

    [Authorize]
    [HttpPost("SaveChatbotMessage")]
    public async Task<IActionResult> SaveChatbotMessage([FromBody] OasisMessage chatbotMessage)
    {
        var chatExists = await _chatService.GetChatByIdAsync(chatbotMessage.OasisChatId!.Value);
        
        if (chatExists is null) 
            return NotFound(OasisApiResult<string>.ErrorResponse("Chat not found"));

        await _chatService.CreateMessageAsync(chatbotMessage);
        
        return StatusCode(201, chatbotMessage);
    }

    private async Task<List<OasisMessage>> CreateThreadAndSendMessageToChatbots(MessageRequestDto messageRequestDto)
    {
        var chatbotsTasks = new List<Task<OasisMessage>>
        {
            _geminiClient.GetChatTitleAsync(messageRequestDto.Message) // Get title from Gemini
        };

        foreach (var chatBot in messageRequestDto.ChatBotEnums)
        {
            switch (chatBot)
            {
                case ChatBotsEnum.ChatGpt:
                    chatbotsTasks.Add(_chatGptClient.CreateChatAndSendMessage(messageRequestDto.Message));
                    break;
                case ChatBotsEnum.Gemini:
                    chatbotsTasks.Add(_geminiClient.CreateChatAndSendMessageAsync(messageRequestDto.Message));
                    break;
                default:
                    throw new OasisException("Invalid chatbot");
            }
        }

        return await ExecuteChatbotTasks(chatbotsTasks);
    }

    private async Task<List<OasisMessage>> SendMessageToChatbotsThreads(IEnumerable<ChatBotsEnum> chatBotEnums, OasisChat chat, string formattedMessage)
    {
        var chatbotsTasks = new List<Task<OasisMessage>>();

        foreach (var chatBot in chatBotEnums)
        {
            switch (chatBot)
            {
                case ChatBotsEnum.ChatGpt:
                    chatbotsTasks.Add(_chatGptClient.SendMessageToChat(chat.ChatGptThreadId!, formattedMessage));
                    break;
                case ChatBotsEnum.Gemini:
                    chatbotsTasks.Add(_geminiClient.SendMessageToChatAsync(chat.Messages));
                    break;
                default:
                    throw new OasisException("Invalid chatbot");
            }
        }

        return await ExecuteChatbotTasks(chatbotsTasks);
    }

    private async Task<List<OasisMessage>> ExecuteChatbotTasks(List<Task<OasisMessage>> chatbotTasks)
    {
        var completedTasks = await Task.WhenAll(chatbotTasks.Select(task => Task.Run(async () =>
        {
            try
            {
                return await task;
            }
            catch (Exception)
            {
                return new OasisMessage(
                    from: "Error",
                    message: "Error processing message, try again later",
                    isSaved: false
                );
            }
        })));

        return completedTasks.ToList();
    }
    
    
    private int GetUserIdFromContext()
    {
        return int.Parse(HttpContext.Items["UserId"]!.ToString()!);
    }
}
