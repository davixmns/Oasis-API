using Domain.Entities;

namespace OasisAPI.Infra.Clients.Interfaces;

public interface IChatBotClient
{
    ChatBotEnum ChatBotEnum { get; }
}