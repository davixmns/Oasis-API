using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OasisAPI.Dto;
using OasisAPI.Interfaces;
using OasisAPI.Interfaces.Services;
using OasisAPI.Models;

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
        if (string.IsNullOrWhiteSpace(messageRequestDto.Message))
        {
            return BadRequest(OasisApiResponse<IActionResult>.ErrorResponse("Message cannot be empty"));
        }

        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Unauthorized(OasisApiResponse<IActionResult>.ErrorResponse("User not found"));
        }

        var userId = int.Parse(userIdClaim);
        var chat = this.unitOfWork.ChatRepository.Create(new OasisChat(
            userId: userId,
            chatGptThreadId: null,
            geminiThreadId: null
        ));

        await this.unitOfWork.CommitAsync();


        return Ok();
    }
}
// // Inicia ambas as tarefas ao mesmo tempo
// var chatGptTask = chatbotsService.StartGptChat(messageRequestDto.Message);
// var geminiTask = chatbotsService.StartGeminiChat(messageRequestDto.Message);
//
// // Espera ambas as tarefas completarem
// await Task.WhenAll(chatGptTask, geminiTask);
//
// // Recupera os resultados das tarefas
// var chatGptResponse = await chatGptTask;
// var geminiResponse = await geminiTask;
//
// return Ok(new {chatGptResponse, geminiResponse});