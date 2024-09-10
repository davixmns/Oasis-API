namespace OasisAPI.App.Config;

public class ChatGptConfig
{
    public string ApiKey { get; set; }
    public string AssistantId { get; set; }
    
    public ChatGptConfig(string apiKey, string assistantId)
    {
        ApiKey = apiKey;
        AssistantId = assistantId;
    }
}