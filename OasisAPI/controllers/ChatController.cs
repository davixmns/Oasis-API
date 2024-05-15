using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OasisAPI.Dto;
using OasisAPI.Enums;
using OasisAPI.Interfaces;
using OasisAPI.Interfaces.Services;
using OasisAPI.Models;
using OasisAPI.Utils;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public sealed class ChatController : ControllerBase
{
    private readonly IChatbotsService _chatbotsService;
    private readonly IUnitOfWork _unitOfWork;

    public ChatController(IUnitOfWork unitOfWork, IChatbotsService chatbotsService)
    {
        _unitOfWork = unitOfWork;
        _chatbotsService = chatbotsService;
    }

    [Authorize]
    [HttpGet("GetAllChats")]
    public async Task<IActionResult> GetAllChats()
    {
        var userId = int.Parse(HttpContext.Items["UserId"]!.ToString()!);

        var chats = await _unitOfWork.ChatRepository
            .GetAll()
            .Where(chat => chat.OasisUserId == userId)
            .Include(chat => chat.Messages)
            .ToListAsync();
        
        return Ok(chats);
    }

    [Authorize]
    [HttpPost("SendFirstMessage")]
    public async Task<IActionResult> SendFirstMessage([FromBody] MessageRequestDto messageRequestDto)
    {
        var tasks = new List<Task<OasisMessage>>
        {
            _chatbotsService.CreateGptChat(messageRequestDto.Message),
            _chatbotsService.CreateGeminiChat(messageRequestDto.Message),
            _chatbotsService.RetrieveChatTheme(messageRequestDto.Message)
        };

        await Task.WhenAll(tasks);

        var chatbotMessages = tasks
            .Select(task => task.Result)
            .ToList();
        
        var chat = _unitOfWork.ChatRepository.Create(new OasisChat(
            oasisUserId: int.Parse(HttpContext.Items["UserId"]!.ToString()!),
            chatGptThreadId: chatbotMessages[0].FromThreadId,
            geminiThreadId: chatbotMessages[1].FromThreadId,
            title: chatbotMessages[2].Message
        ));

        await _unitOfWork.CommitAsync();
        
        _unitOfWork.MessageRepository.Create(new OasisMessage(
            from: "User",
            message: messageRequestDto.Message,
            oasisChatId: chat.OasisChatId,
            isSaved: true
        ));

        await _unitOfWork.CommitAsync();
        
        return StatusCode(201, new
        {
            chat,
            chatbotMessages,
        });
    }

    [Authorize]
    [HttpPost("SendMessage/{oasisChatId:int}")]
    public async Task<IActionResult> SendMessageToChat(int oasisChatId, [FromBody] MessageRequestDto messageRequestDto)
    {
        var chatExists = await _unitOfWork.ChatRepository.GetAsync(c => c.OasisChatId == oasisChatId);

        if (chatExists is null)
            return NotFound("This chat does not exist");
        
        _unitOfWork.MessageRepository.Create(new OasisMessage(
            from: "User",
            message: messageRequestDto.Message,
            oasisChatId: chatExists.OasisChatId
        ));

        await _unitOfWork.CommitAsync();

        var chatMessages = await _unitOfWork.MessageRepository
            .GetAll()
            .Where(m => m.OasisChatId == oasisChatId)
            .ToListAsync();
        
        var lastestChatbotMessage = chatMessages
            .Where(m => m.From != "User")
            .MaxBy(m => m.CreatedAt)!
            .Message;
        
        var formattedMessageToGpt = OasisMessageFormatter
            .FormatToChatbotAndUserMessage(lastestChatbotMessage, messageRequestDto.Message);
        
        var chatbotTasks = new List<Task<OasisMessage>>();
        
        foreach (var chatBot in messageRequestDto.ChatBotEnums)
        {
            switch (chatBot)
            {
                case ChatBotEnum.ChatGpt:
                    chatbotTasks.Add(_chatbotsService.SendMessageToGptChat(chatExists.ChatGptThreadId!, formattedMessageToGpt));
                    break;
                case ChatBotEnum.Gemini:
                    chatbotTasks.Add(_chatbotsService.SendMessageToGeminiChat(chatMessages));
                    break;
                default:
                    return BadRequest("Invalid chatbot");
            }
        }

        await Task.WhenAll(chatbotTasks);
        
        var chatbotMessages = chatbotTasks
            .Select(task => task.Result)
            .ToList();

        return Ok(chatbotMessages);
    }

    [Authorize]
    [HttpPost("SaveChatbotMessage")]
    public async Task<IActionResult> SaveChatbotMessage([FromBody] OasisMessage chatbotMessage)
    {
        var chatExists = await _unitOfWork.ChatRepository
            .GetAsync(c => c.OasisChatId == chatbotMessage.OasisChatId);
        
        if (chatExists is null) 
            return NotFound("This chat does not exist");
        
        _unitOfWork.MessageRepository.Create(chatbotMessage);

        await _unitOfWork.CommitAsync();

        return StatusCode(201, chatbotMessage);
    }
}