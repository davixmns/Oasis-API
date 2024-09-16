using Domain.Entities;
using Domain.Utils;

namespace OasisAPI.Infra.Clients.Interfaces;

public interface IChatBotClient
{
    ChatBotEnum ChatBotEnum { get; }
}