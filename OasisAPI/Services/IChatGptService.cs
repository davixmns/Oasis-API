using Microsoft.AspNetCore.Mvc;

namespace OasisAPI.Services;

public interface IChatGptService
{
    Task<IActionResult> CreateThreadSendMessageAndRun(string userMessage);
    Task<IActionResult> SendMessageToThread(string threadId, string userMessage);
    Task<IActionResult> RetrieveMessageList(string threadId);
    Task<IActionResult> RetrieveMessage(string messageId, string threadId);
}