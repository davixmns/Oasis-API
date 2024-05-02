using System.ComponentModel.DataAnnotations;
using OasisAPI.Enums;

namespace OasisAPI.Dto;

public class MessageRequestDto
{
    [Required]
    public string Message { get; set; }
    [Required]
    public HashSet<ChatBotEnum> ChatBotEnums { get; set; }
}