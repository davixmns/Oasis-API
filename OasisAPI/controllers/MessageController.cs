using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using OasisAPI.Interfaces;
using OasisAPI.Models;
using OasisAPI.Services;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly IChatGptService _chatGptService;
    private readonly IGeminiService _geminiService;
    
    public MessageController(IChatGptService chatGptService, IGeminiService geminiService)
    {
        _chatGptService = chatGptService;
        _geminiService = geminiService;
    }
    
    [HttpPost("SendFirstMessage")]
    public async Task<IActionResult> SendFirstMessage([FromBody] string userMessage)
    {
        if (string.IsNullOrEmpty(userMessage))
            return BadRequest("User message cannot be empty.");

        // Inicia ambas as tarefas ao mesmo tempo
        var chatGptTask = _chatGptService.StartChat(userMessage);
        var geminiTask = _geminiService.StartChat(userMessage);
        
        // Espera ambas as tarefas completarem
        await Task.WhenAll(chatGptTask, geminiTask);

        // Recupera os resultados das tarefas
        var chatGptResponse = await chatGptTask;
        var geminiResponse = await geminiTask;
        
        return Ok(new {chatGptResponse, geminiResponse});
    }
}