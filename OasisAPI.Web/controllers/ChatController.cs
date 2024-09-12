using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OasisAPI.App.Commands;
using OasisAPI.App.Dto.Request;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public sealed class ChatController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly int _userId;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
        _userId = HttpContext.Items["UserId"] as int? 
                  ?? throw new Exception("User id not found in context");
    }

    [Authorize]
    [HttpGet("GetAllUserChats")]
    public async Task<IActionResult> GetAllUserChats()
    {
        var command = new GetAllUserChatsQuery(_userId);

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }

    [Authorize]
    [HttpPost("StartConversation")]
    public async Task<IActionResult> StartConversation([FromBody] CreateChatRequestDto dto)
    {
        var createChatCommand = new CreateChatCommand(_userId, "Untitled", dto.Message);

        var createdChatResult = await _mediator.Send(createChatCommand);

        if (!createdChatResult.IsSuccess)
            return BadRequest(createdChatResult);

        var startConversationCommand = new StartConversationWithChatBotsCommand(dto.Message, dto.ChatBotEnums);

        var messagesResult = await _mediator.Send(startConversationCommand);

        if (!messagesResult.IsSuccess)
            return BadRequest(messagesResult);

        var response = new
        {
            Chat = createdChatResult.Data,
            Messages = messagesResult.Data!.Skip(1)
        };

        return Ok(response);
    }

    [Authorize]
    [HttpPost("SendMessage/{oasisChatId:int}")]
    public async Task<IActionResult> SendMessageToChat(int oasisChatId, CreateChatRequestDto createChatRequestDto)
    {
        return Ok();
        // var chat = await _chatService.GetChatByIdAsync(oasisChatId);
        //
        // if (chat is null) 
        //     return NotFound(AppResult<string>.ErrorResponse("Chat not found"));
        //
        // await _chatService.CreateMessageAsync(new OasisMessage(
        //     from: "User",
        //     message: createChatRequestDto.Message,
        //     oasisChatId: oasisChatId,
        //     isSaved: true
        // ));
        //
        // var chatMessages = await _chatService.GetMessagesByChatId(oasisChatId);
        // var latestChatbotMessage = chatMessages.LastOrDefault(m => m.From != "User")?.Message;
        //
        // var formattedMessageToGpt = OasisMessageFormatter.FormatToChatbotAndUserMessage(latestChatbotMessage!, createChatRequestDto.Message);
        //
        // var chatbotMessages = await SendMessageToChatbotsThreads(createChatRequestDto.ChatBotEnums, chat, formattedMessageToGpt);
        //
        // return Ok(AppResult<List<OasisMessage>>.SuccessResponse(chatbotMessages));
    }

    [Authorize]
    [HttpPost("SaveChatbotMessage")]
    public async Task<IActionResult> SaveChatbotMessage([FromBody] OasisMessage chatbotMessage)
    {
        return Ok();

        // var chatExists = await _chatService.GetChatByIdAsync(chatbotMessage.OasisChatId!.Value);
        //
        // if (chatExists is null) 
        //     return NotFound(AppResult<string>.ErrorResponse("Chat not found"));
        //
        // await _chatService.CreateMessageAsync(chatbotMessage);
        //
        // return StatusCode(201, chatbotMessage);
    }
}