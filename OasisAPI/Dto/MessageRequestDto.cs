using System.ComponentModel.DataAnnotations;
using OasisAPI.Enums;

namespace OasisAPI.Dto;

public class MessageRequestDto
{
    [Required]
    [MinLength(1)]
    public string Message { get; set; }
    
    
    [Required]
    [MinLength(1)] //min of 1 chatbot
    [MaxLength(2)] //max of 2 chatbots
    public HashSet<ChatBotEnum> ChatBotEnums { get; set; }
}