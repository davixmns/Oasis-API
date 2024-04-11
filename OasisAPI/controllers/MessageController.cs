using Microsoft.AspNetCore.Mvc;
using OasisAPI.Interfaces;
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
    
    [HttpPost("Thread")]
    public async Task<IActionResult> CreateThread([FromBody] string userMessage)
    {
        if(userMessage == "")
            return BadRequest("User message cannot be empty.");
        
        // var chatGptResponse = await _chatGptService.CreateThreadSendMessageAndRun(userMessage);

        var geminiResponse = await _geminiService.StartChat(userMessage);
        
        return Created("", geminiResponse);
    }
    
    [HttpPost("Thread/{threadId}")]
    public async Task<IActionResult> SendMessageToThread(string threadId, [FromBody] string userMessage)
    {
        var response = await _chatGptService.SendMessageToThread(threadId, userMessage);
        return Ok(response);
    }
    
    [HttpGet("Thread/{threadId}")]
    public async Task<IActionResult> RetrieveMessageList(string threadId)
    {
        var response = await _chatGptService.RetrieveMessageList(threadId);
        return Ok(response);
    }
    
    
    [HttpGet("Thread/{threadId}/{messageId}")]
    public async Task<IActionResult> RetrieveMessage(string threadId, string messageId)
    {
        var response = await _chatGptService.RetrieveMessage(threadId, messageId);
        return Ok(response);
    }
}