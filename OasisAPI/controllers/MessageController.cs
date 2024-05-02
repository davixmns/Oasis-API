using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
public class MessageController : ControllerBase
{
    private readonly IChatbotsService chatbotsService;
    private readonly IUnitOfWork unitOfWork;

    public MessageController(IUnitOfWork unitOfWork, IChatbotsService chatbotsService)
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
            chatbotsService.StartGeminiChat(formatedUserMessage)
        };
        await Task.WhenAll(tasks);

        var chatbotMessages = tasks.Select(task => task.Result).ToList();

        var chat = this.unitOfWork.ChatRepository.Create(new OasisChat(
            userId: int.Parse(HttpContext.Items["UserId"]!.ToString()!),
            chatGptThreadId: chatbotMessages[0].FromThreadId,
            geminiThreadId: chatbotMessages[1].FromThreadId
        ));
        await this.unitOfWork.CommitAsync();

        var userMessage = this.unitOfWork.MessageRepository.Create(new OasisMessage(
            from: "User",
            message: messageRequestDto.Message,
            oasisChatId: chat.OasisChatId
        ));
        await this.unitOfWork.CommitAsync();
        
        return StatusCode(201, new
        {
            chat,
            userMessage,
            chatbotMessages
        });
    }
}