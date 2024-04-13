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
    private readonly IChatbotService _chatGptService;
    private readonly IChatbotService _geminiService;
    
    public MessageController(IChatbotService chatGptService, IChatbotService geminiService)
    {
        _chatGptService = chatGptService;
        _geminiService = geminiService;
    }
    
    [HttpPost("Thread")]
    public async Task<IActionResult> CreateThread([FromBody] string userMessage)
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