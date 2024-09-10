namespace OasisAPI.App.Config;

public class GeminiConfig
{
    public string ApiKey { get; set; }
    public string Model { get; set; }
    
    public GeminiConfig(string apiKey, string model)
    {
        ApiKey = apiKey;
        Model = model;
    }
}