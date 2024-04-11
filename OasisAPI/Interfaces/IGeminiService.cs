using Microsoft.AspNetCore.Mvc;

namespace OasisAPI.Interfaces;

public interface IGeminiService
{
    public Task<IActionResult> StartChat(string userMessage);
}