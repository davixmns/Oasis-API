using Microsoft.AspNetCore.Mvc;

namespace OasisAPI.Services;

public interface IChatGptService
{
    // Task<string> CreateThread();
    //
    // Task<string> SendMessageToThread(string threadId, string userMessage);

    Task<IActionResult> Test();
    Task<IActionResult> CreateThread(string userMessage);
    Task<IActionResult> SendMessageToThread(string threadId, string userMessage);
    Task<IActionResult> RetrieveMessageList(string threadId);
    Task<IActionResult> RetrieveMessage(string messageId, string threadId);
}