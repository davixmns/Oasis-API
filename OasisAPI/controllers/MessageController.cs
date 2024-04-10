using Microsoft.AspNetCore.Mvc;
using OasisAPI.Services;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly ILogger<MessageController> _logger;
    private readonly IChatGptService _chatGptService;
    
    public MessageController(ILogger<MessageController> logger, IChatGptService chatGptService)
    {
        _logger = logger;
        _chatGptService = chatGptService;
    }
    
    [HttpPost("Thread")]
    public async Task<IActionResult> CreateThread([FromBody] string userMessage)
    {
        if(userMessage == "")
            return BadRequest("User message cannot be empty.");
        
        string formattedMessage = "<NOVA MENSAGEM DO USUÁRIO>" + '\n' + userMessage;
        var response = await _chatGptService.CreateThread(formattedMessage);
        return Created("", response);
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