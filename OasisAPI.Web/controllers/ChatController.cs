using AutoMapper;
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

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet("GetAllUserChats")]
    public async Task<IActionResult> GetAllUserChats()
    {
        var command = new GetAllUserChatsQuery(GetUserIdFromContext());

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }

    [Authorize]
    [HttpPost("StartConversation")]
    public async Task<IActionResult> StartConversationWithChatBots(StartConversationRequestDto dto)
    {
        var createOasisChatCommand = new CreateOasisChatCommand(GetUserIdFromContext(), "Untitled", dto.Message);
        var createdOasisChatResult = await _mediator.Send(createOasisChatCommand);

        if (!createdOasisChatResult.IsSuccess)
            return BadRequest(createdOasisChatResult);

        var startConversationCommand = new StartConversationWithChatBotsCommand(dto.Message, dto.ChatBotEnums);
        var messagesResult = await _mediator.Send(startConversationCommand);

        if (!messagesResult.IsSuccess)
            return BadRequest(messagesResult);

        var updateOasisChatCommand = new UpdateOasisChatDetailsCommand(createdOasisChatResult.Data!, messagesResult.Data!);
        var updatedChatResult = await _mediator.Send(updateOasisChatCommand);

        if (!updatedChatResult.IsSuccess)
            return BadRequest(updatedChatResult);

        var response = new
        {
            OasisChat = createdOasisChatResult.Data,
            ChatBotMessages = messagesResult.Data!.Skip(1)
        };

        return Ok(response);
    }

    [Authorize]
    [HttpPost("ContinueConversation")]
    public async Task<IActionResult> SendMessageToChat(ContinueConversationRequestDto dto)
    {
        var createMessageCommand = new CreateOasisMessageCommand(dto.OasisChatId, dto.Message, ChatBotEnum.User);
        var createMessageResult = await _mediator.Send(createMessageCommand);
        
        if (!createMessageResult.IsSuccess)
            return BadRequest(createMessageResult);
        
        var continueConversationCommand = new ContinueConversationWithChatBotsCommand(dto.OasisChatId, dto.Message, dto.ChatBotEnums);
        var chatBotMessagesResult = await _mediator.Send(continueConversationCommand);
        
        if (!chatBotMessagesResult.IsSuccess)
            return BadRequest(chatBotMessagesResult);
        
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

    private int GetUserIdFromContext()
    {
        return HttpContext.Items["UserId"] as int? ?? throw new Exception("User Id not found");
    }
}