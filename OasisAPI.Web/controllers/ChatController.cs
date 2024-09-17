using Domain.Entities;
using Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OasisAPI.App.Features.Chat.Commands.CreateOasisChat;
using OasisAPI.App.Features.Chat.Commands.UpdateOasisChatDetails;
using OasisAPI.App.Features.Chat.Queries.GetAllUserChats;
using OasisAPI.App.Features.ChatBot.Commands.ContinueConversationWithChatBots;
using OasisAPI.App.Features.ChatBot.Commands.StartConversationWithChatBots;
using OasisAPI.App.Features.Message.Commands.CreateOasisMessage;

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
    public async Task<IActionResult> StartConversationWithChatBots(StartConversationWithChatBotsDto dto)
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
    public async Task<IActionResult> ContinueConversationWithChatBots(ContinueConversationRequestDto dto)
    {
        var createMessageCommand = new CreateOasisMessageCommand(dto.OasisChatId, dto.Message, ChatBotEnum.User);
        var createMessageResult = await _mediator.Send(createMessageCommand);
        
        if (!createMessageResult.IsSuccess)
            return BadRequest(createMessageResult);
        
        var continueConversationCommand = new ContinueConversationWithChatBotsCommand(dto.OasisChatId, dto.Message, dto.ChatBotEnums);
        var chatBotMessagesResult = await _mediator.Send(continueConversationCommand);
        
        return chatBotMessagesResult.IsSuccess 
            ? Ok(chatBotMessagesResult)
            : BadRequest(chatBotMessagesResult);
    }

    [Authorize]
    [HttpPost("SaveChatBotMessage")]
    public async Task<IActionResult> SaveChatBotMessage(CreateOasisMessageRequestDto dto)
    {
        var command = new CreateOasisMessageCommand(dto.OasisChatId, dto.Message, dto.ChatBotEnum);

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }

    private int GetUserIdFromContext()
    {
        return HttpContext.Items["UserId"] as int? ?? throw new Exception("User Id not found");
    }
}