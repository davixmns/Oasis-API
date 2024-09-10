namespace OasisAPI.App.Utils;

public static class OasisMessageFormatter
{
    public static string FormatToFirstUserMessage(string message)
    {
        return $"<INICIO DA PRIMEIRA MENSAGEM DO USUÁRIO>" +
               $"\n " +
               $"{message}" +
               $"\n " +
               $"</FIM>";
    }
    
    public static string FormatToUserMessage(string message)
    {
        return $"<INICIO DA MENSAGEM DO USUÁRIO>" +
               $"\n " +
               $"{message}" +
               $"\n " +
               $"</FIM>";
    }
    
    public static string FormatToChatbotMessage(string message)
    {
        return $"<INICIO DA MENSAGEM DA IA QUE MAIS AGRADOU O USUÁRIO>" +
               $"\n " +
               $"{message}" +
               $"\n " +
               $"</FIM>";
    }
    
    public static string FormatToChatbotAndUserMessage(string chatbotMessage, string userMessage)
    {
        return $"<INICIO DA MENSAGEM DA IA QUE MAIS AGRADOU O USUÁRIO>" +
               $"\n " +
               $"{chatbotMessage}" +
               $"\n " +
               $"</FIM>" +
               $"\n " +
               $"<INICIO DA MENSAGEM DO USUÁRIO>" +
               $"\n " +
               $"{userMessage}" +
               $"\n " +
               $"</FIM>";
    }

    public static string FormatToGetChatTitle(string message)
    {
        return "Preciso que voce me diga como seria o titulo dessa mensagem," +
               "use no minimo 1 palavra e no maximo 3, precisa ser um titulo curto: " + message;
    }
}