using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
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
    [HttpPost("SendFirstMessage")]
    public async Task<IActionResult> SendFirstMessage([FromBody] MessageRequestDto messageRequestDto)
    {
        var formatedUserMessage = UserMessageFormatter.FormatToFirstUserMessage(messageRequestDto.Message);
        
        var tasks = new List<Task<OasisMessage>>
        {
            chatbotsService.StartGptChat(formatedUserMessage),
            chatbotsService.StartGeminiChat(formatedUserMessage),
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

        var userMessage = this.unitOfWork.MessageRepository.Create(new OasisMessage(
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
    [HttpPost("SaveChatbotMessage")]
    public async Task<IActionResult> SaveChatbotMessage([FromBody] OasisMessage chatbotMessage)
    {
        var chatExists = await this.unitOfWork
            .ChatRepository
            .GetAsync(c => c.OasisChatId == chatbotMessage.OasisChatId)
            .ConfigureAwait(false);

        if (chatExists is null)
        {
            return NotFound(OasisApiResponse<string>.ErrorResponse("This chat does not exist"));
        }
        
        this.unitOfWork.MessageRepository.Create(chatbotMessage);
        
        await this.unitOfWork.CommitAsync();
        
        return StatusCode(201, chatbotMessage);
    }
}