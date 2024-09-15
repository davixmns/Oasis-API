namespace Domain.Entities;

public class OasisChatBotDetails : BaseEntity
{
    public int OasisChatId { get; set; }        // ID do chat
    public ChatBotEnum ChatBotEnum { get; set; }   // Tipo do chatbot
    public bool IsSelected { get; set; }       // Se o chatbot foi selecionado
    public string? ThreadId { get; set; }      // Thread ID específico do chatbot
    
    public OasisChatBotDetails(int oasisChatId, ChatBotEnum chatBotEnum, bool isSelected, string? threadId)
    {
        OasisChatId = oasisChatId;
        ChatBotEnum = chatBotEnum;
        IsSelected = isSelected;
        ThreadId = threadId;
    }
}
