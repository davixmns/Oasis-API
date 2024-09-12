using Domain.Entities;
using Domain.utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OasisAPI.App.Commands;
using OasisAPI.App.Dto.Request;
using OasisAPI.App.Exceptions;
using OasisAPI.App.Utils;
using OasisAPI.Enums;
using OasisAPI.Infra.Clients;
using OasisAPI.Infra.Dto;
using OasisAPI.Interfaces.Services;
using OasisAPI.Models;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public sealed class ChatController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IChatService _chatService;
    private readonly IChatGptClient _chatGptClient;
    private readonly IGeminiClient _geminiClient;

    public ChatController(IMediator mediator, IChatService chatService, IChatGptClient chatGptClient, IGeminiClient geminiClient)
    {
        _chatService = chatService;
        _chatGptClient = chatGptClient;
        _geminiClient = geminiClient;
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet("GetAllUserChats")]
    public async Task<IActionResult> GetAllUserChats()
    {
        var userId = GetUserIdFromContext();
        
        var command = new GetAllUserChatsQuery(userId);
        
        var result = await _mediator.Send(command);
        
        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }

    [Authorize]
    [HttpPost("StartConversation")]
    public async Task<IActionResult> StartConversation([FromBody] CreateChatRequestDto dto)
    {
        var userId = GetUserIdFromContext();
        
        var startConversationCommand = new StartConversationWithChatBotsCommand(
            message: dto.Message,
            chatBotsEnums: dto.ChatBotEnums
        );
        
        var receivedMessagesResult = await _mediator.Send(startConversationCommand);
        
        if(!receivedMessagesResult.IsSuccess)
            return BadRequest(receivedMessagesResult);

        var msgData = receivedMessagesResult.Data!.ToList();
        var createChatCommand = new CreateChatCommand(
            oasisUserId: userId,
            title: msgData!.FirstOrDefault()?.Message ?? "Untitled",
            chatGptThreadId: msgData.FirstOrDefault(m => m.From == FromNames.ChatGpt).FromThreadId,
            geminiThreadId: msgData.FirstOrDefault(m => m.From == FromNames.Gemini).FromThreadId
        );
        
        var createdChatResult = await _mediator.Send(createChatCommand);
        
        if (!createdChatResult.IsSuccess)
            return BadRequest(createdChatResult);

        var chatData = createdChatResult.Data!;
        var createMessageCommand = new CreateMessageCommand(
            oasisChatId: chatData.Id,
            message: dto.Message,
            from: FromNames.User,
            isSaved: true
        );

        var createdMessageResult = await _mediator.Send(createMessageCommand);
        
        if (!createdMessageResult.IsSuccess)
            return BadRequest(createdMessageResult);

        var response = new
        {
            chat = chatData,
            chatbotMessages = msgData.Skip(1)
        };

        return StatusCode(StatusCodes.Status201Created, AppResult<object>.SuccessResponse(response));
    }

    [Authorize]
    [HttpPost("SendMessage/{oasisChatId:int}")]
    public async Task<IActionResult> SendMessageToChat(int oasisChatId, [FromBody] CreateChatRequestDto createChatRequestDto)
    {
        var chat = await _chatService.GetChatByIdAsync(oasisChatId);
        
        if (chat is null) 
            return NotFound(AppResult<string>.ErrorResponse("Chat not found"));

        await _chatService.CreateMessageAsync(new OasisMessage(
            from: "User",
            message: createChatRequestDto.Message,
            oasisChatId: oasisChatId,
            isSaved: true
        ));

        var chatMessages = await _chatService.GetMessagesByChatId(oasisChatId);
        var latestChatbotMessage = chatMessages.LastOrDefault(m => m.From != "User")?.Message;

        var formattedMessageToGpt = OasisMessageFormatter.FormatToChatbotAndUserMessage(latestChatbotMessage!, createChatRequestDto.Message);
        
        var chatbotMessages = await SendMessageToChatbotsThreads(createChatRequestDto.ChatBotEnums, chat, formattedMessageToGpt);

        return Ok(AppResult<List<OasisMessage>>.SuccessResponse(chatbotMessages));
    }

    [Authorize]
    [HttpPost("SaveChatbotMessage")]
    public async Task<IActionResult> SaveChatbotMessage([FromBody] OasisMessage chatbotMessage)
    {
        var chatExists = await _chatService.GetChatByIdAsync(chatbotMessage.OasisChatId!.Value);
        
        if (chatExists is null) 
            return NotFound(AppResult<string>.ErrorResponse("Chat not found"));

        await _chatService.CreateMessageAsync(chatbotMessage);
        
        return StatusCode(201, chatbotMessage);
    }
    
    private int GetUserIdFromContext()
    {
        return int.Parse(HttpContext.Items["UserId"]!.ToString()!);
    }
}
