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
    private readonly ITokenService tokenService;
    
    public MessageController(IChatbotsService chatbotsService, ITokenService tokenService)
    {
        this.chatbotsService = chatbotsService;
        this.tokenService = tokenService;
    }
    
    [Authorize]
    [HttpPost("SendFirstMessage")]
    public async Task<IActionResult> SendFirstMessage([FromBody] MessageRequestDto messageRequestDto)
    {
        if (string.IsNullOrWhiteSpace(messageRequestDto.Message))
        {
            return BadRequest(OasisApiResponse<IActionResult>
                .ErrorResponse("Message cannot be empty"));
        }
        
        // Inicia ambas as tarefas ao mesmo tempo
        var chatGptTask = chatbotsService.StartGptChat(messageRequestDto.Message);
        var geminiTask = chatbotsService.StartGeminiChat(messageRequestDto.Message);
        
        // Espera ambas as tarefas completarem
        await Task.WhenAll(chatGptTask, geminiTask);

        // Recupera os resultados das tarefas
        var chatGptResponse = await chatGptTask;
        var geminiResponse = await geminiTask;
        
        return Ok(new {chatGptResponse, geminiResponse});
    }
}