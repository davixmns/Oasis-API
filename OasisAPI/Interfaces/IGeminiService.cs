using Microsoft.AspNetCore.Mvc;
using OasisAPI.Models;

namespace OasisAPI.Interfaces;

public interface IGeminiService
{
    public Task<OasisMessage> StartChat(string userMessage);
}