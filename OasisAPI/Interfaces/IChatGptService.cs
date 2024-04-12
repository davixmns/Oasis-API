using Microsoft.AspNetCore.Mvc;
using OasisAPI.Models;

namespace OasisAPI.Interfaces;

public interface IChatGptService
{
    Task<OasisMessage> CreateThreadSendMessageAndRun(string userMessage);
    Task<IActionResult> SendMessageToThread(string threadId, string userMessage);
    Task<IActionResult> RetrieveMessageList(string threadId);
    Task<IActionResult> RetrieveMessage(string messageId, string threadId);
}