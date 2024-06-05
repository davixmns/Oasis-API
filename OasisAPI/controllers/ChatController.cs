using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OasisAPI.Dto;
using OasisAPI.Enums;
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
        var userId = int.Parse(HttpContext.Items["UserId"]!.ToString()!);
        var chatsResponse = await _chatService.GetAllChatsAsync(userId);
        return Ok(chatsResponse);
    }

    [Authorize]
    [HttpPost("SendFirstMessage")]
    public async Task<IActionResult> SendFirstMessage([FromBody] MessageRequestDto messageRequestDto)
    {
        var tasks = new List<Task<OasisMessage>>
        {
            _chatGptClient.CreateChatAndSendMessage(messageRequestDto.Message),
            _geminiClient.CreateChatAndSendMessageAsync(messageRequestDto.Message),
            _geminiClient.GetChatTitleAsync(messageRequestDto.Message)
        };

        await Task.WhenAll(tasks);

        var chatbotMessages = tasks.Select(task => task.Result).ToList();
        
        var createdChat = await _chatService.CreateChatAsync(new OasisChat(
            oasisUserId: int.Parse(HttpContext.Items["UserId"]!.ToString()!),
            chatGptThreadId: chatbotMessages[0].FromThreadId,
            geminiThreadId: chatbotMessages[1].FromThreadId,
            title: chatbotMessages[2].Message
        ));
        
        await _chatService.CreateMessageAsync(new OasisMessage(
            from: "User",
            message: messageRequestDto.Message,
            oasisChatId: createdChat.OasisChatId,
            isSaved: true
        ));

        return StatusCode(201, new { chat = createdChat, chatbotMessages });
    }

    [Authorize]
    [HttpPost("SendMessage/{oasisChatId:int}")]
    public async Task<IActionResult> SendMessageToChat(int oasisChatId, [FromBody] MessageRequestDto messageRequestDto)
    {
        var chat = await _chatService.GetChatById(oasisChatId);

        if (chat is null)
            return NotFound("This chat does not exist");

        await _chatService.CreateMessageAsync(new OasisMessage(
            from: "User",
            message: messageRequestDto.Message,
            oasisChatId: oasisChatId,
            isSaved: true
        ));

        var chatMessages = await _chatService.GetMessagesByChatId(oasisChatId);
        
        var latestChatbotMessage = chatMessages.Last(m => m.From != "User").Message;
        
        var formattedMessageToGpt = OasisMessageFormatter
            .FormatToChatbotAndUserMessage(latestChatbotMessage, messageRequestDto.Message);
        
        var chatbotsTasks = new List<Task<OasisMessage>>();

        foreach (var chatBot in messageRequestDto.ChatBotEnums)
        {
            switch (chatBot)
            {
                case ChatBotEnum.ChatGpt:
                    chatbotsTasks.Add(_chatGptClient.SendMessageToChat(chat.ChatGptThreadId!, formattedMessageToGpt));
                    break;
                case ChatBotEnum.Gemini:
                    chatbotsTasks.Add(_geminiClient.SendMessageToChatAsync(chatMessages));
                    break;
                default:
                    return BadRequest("Invalid chatbot");
            }
        }

        await Task.WhenAll(chatbotsTasks);

        var chatbotMessages = chatbotsTasks.Select(task => task.Result).ToList();

        return Ok(chatbotMessages);
    }

    [Authorize]
    [HttpPost("SaveChatbotMessage")]
    public async Task<IActionResult> SaveChatbotMessage([FromBody] OasisMessage chatbotMessage)
    {
        var chatExists = await _chatService.GetChatById(chatbotMessage.OasisChatId!.Value);

        if (chatExists is null)
            return NotFound("This chat does not exist");

        await _chatService.CreateMessageAsync(chatbotMessage);

        return StatusCode(201, chatbotMessage);
    }
}