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
public class ChatController : ControllerBase
{
    private readonly IChatbotsService chatbotsService;
    private readonly IUnitOfWork unitOfWork;

    public ChatController(IUnitOfWork unitOfWork, IChatbotsService chatbotsService)
    {
        this.unitOfWork = unitOfWork;
        this.chatbotsService = chatbotsService;
    }

    [Authorize]
    [HttpGet("GetAllChats")]
    public async Task<IActionResult> GetAllChats()
    {
        var userId = int.Parse(HttpContext.Items["UserId"]!.ToString()!);

        var chats = await this.unitOfWork.ChatRepository
            .GetAll()
            .Where(chat => chat.UserId == userId)
            .Include(chat => chat.Messages)
            .ToListAsync()
            .ConfigureAwait(false);

        return Ok(chats);
    }

    [Authorize]
    [HttpPost("SendFirstMessage")]
    public async Task<IActionResult> SendFirstMessage([FromBody] MessageRequestDto messageRequestDto)
    {
        var formatedUserMessage = OasisMessageFormatter.FormatToFirstUserMessage(messageRequestDto.Message);

        var tasks = new List<Task<OasisMessage>>
        {
            chatbotsService.CreateGptChat(formatedUserMessage),
            chatbotsService.CreateGeminiChat(formatedUserMessage),
            chatbotsService.RetrieveChatTheme(messageRequestDto.Message)
        };

        await Task
            .WhenAll(tasks)
            .ConfigureAwait(false);

        var chatbotMessages = tasks
            .Select(task => task.Result)
            .ToList();

        var chat = this.unitOfWork.ChatRepository.Create(new OasisChat(
            userId: int.Parse(HttpContext.Items["UserId"]!.ToString()!),
            chatGptThreadId: chatbotMessages[0].FromThreadId,
            geminiThreadId: chatbotMessages[1].FromThreadId,
            title: chatbotMessages[2].Message
        ));

        await this.unitOfWork
            .CommitAsync()
            .ConfigureAwait(false);

        _ = this.unitOfWork.MessageRepository.Create(new OasisMessage(
            from: "User",
            message: messageRequestDto.Message,
            oasisChatId: chat.OasisChatId,
            isSaved: true
        ));

        await this.unitOfWork
            .CommitAsync()
            .ConfigureAwait(false);

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
        var chatExists = await this.unitOfWork
            .ChatRepository
            .GetAsync(c => c.OasisChatId == oasisChatId)
            .ConfigureAwait(false);

        if (chatExists is null)
        {
            return NotFound("This chat does not exist");
        }

        var userMessage = messageRequestDto.Message;

        this.unitOfWork.MessageRepository.Create(new OasisMessage(
            from: "User",
            message: userMessage,
            oasisChatId: chatExists.OasisChatId
        ));

        await this.unitOfWork
            .CommitAsync()
            .ConfigureAwait(false);
        
        var chatMessages = await this.unitOfWork
            .MessageRepository
            .GetAll()
            .Where(m => m.OasisChatId == oasisChatId)
            .ToListAsync()
            .ConfigureAwait(false);
        
        var choosedChatbotMessage = chatMessages
            .Where(m => m.From != "User")
            .MaxBy(m => m.CreatedAt)!
            .Message;
        var formattedUserMessage = OasisMessageFormatter.FormatToUserMessage(userMessage);
        var formattedChatbotMessage = OasisMessageFormatter.FormatToChatbotMessage(choosedChatbotMessage);
        var formattedMessage =  formattedChatbotMessage + "\n" + formattedUserMessage;
        
        var tasks = new List<Task<OasisMessage>>();
        foreach (var chatBot in messageRequestDto.ChatBotEnums)
        {
            switch (chatBot)
            {
                case ChatBotEnum.ChatGpt:
                    tasks.Add(chatbotsService.SendMessageToGptChat(chatExists.ChatGptThreadId!, formattedMessage));
                    break;
                case ChatBotEnum.Gemini:
                    tasks.Add(chatbotsService.SendMessageToGeminiChat(chatMessages));
                    break;
                default:
                    return BadRequest("Invalid chatbot");
            }
        }

        await Task
            .WhenAll(tasks)
            .ConfigureAwait(false);
        
        var chatbotMessages = tasks
            .Select(task => task.Result)
            .ToList();

        return Ok(chatbotMessages);
    }

    [Authorize]
    [HttpPost("SaveChatbotMessage")]
    public async Task<IActionResult> SaveChatbotMessage([FromBody] OasisMessage chatbotMessage)
    {
        var chatExists = await this.unitOfWork
            .ChatRepository
            .GetAsync(c => c.OasisChatId == chatbotMessage.OasisChatId)
            .ConfigureAwait(false);

        if (chatExists is null)
        {
            return NotFound("This chat does not exist");
        }

        this.unitOfWork.MessageRepository.Create(chatbotMessage);

        await this.unitOfWork
            .CommitAsync()
            .ConfigureAwait(false);

        return StatusCode(201, chatbotMessage);
    }
}