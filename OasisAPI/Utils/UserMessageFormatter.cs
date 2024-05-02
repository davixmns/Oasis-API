namespace OasisAPI.Utils;

public static class UserMessageFormatter
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
}