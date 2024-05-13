namespace OasisAPI.Utils;

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
}