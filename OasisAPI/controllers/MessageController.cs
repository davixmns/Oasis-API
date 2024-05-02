using Microsoft.AspNetCore.Mvc;
using OasisAPI.Interfaces;
using OasisAPI.Interfaces.Services;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly IChatbotsService _chatbotsService;
    
    public MessageController(IChatbotsService chatbotsService)
    {
        _chatbotsService = chatbotsService;
    }
    
    [HttpPost("SendFirstMessage")]
    public async Task<IActionResult> SendFirstMessage([FromBody] string userMessage)
    {
        if (string.IsNullOrEmpty(userMessage))
            return BadRequest("User message cannot be empty.");

        // Inicia ambas as tarefas ao mesmo tempo
        var chatGptTask = _chatbotsService.StartGptChat(userMessage);
        var geminiTask = _chatbotsService.StartGeminiChat(userMessage);
        
        // Espera ambas as tarefas completarem
        await Task.WhenAll(chatGptTask, geminiTask);

        // Recupera os resultados das tarefas
        var chatGptResponse = await chatGptTask;
        var geminiResponse = await geminiTask;
        
        return Ok(new {chatGptResponse, geminiResponse});
    }
}