using OasisAPI.Models;

namespace OasisAPI.Builders;

public class OasisMessageBuilder : IMyBuilder<OasisMessage>
{
    private string From;
    private string Message;
    private string? FromMessageId;

    public OasisMessageBuilder SetName(string name)
    {
        From = name;
        return this;
    }

    public OasisMessageBuilder SetMessage(string message)
    {
        Message = message;
        return this;
    }

    public OasisMessageBuilder SetMessageId(string? messageId)
    {
        this.FromMessageId = messageId;
        return this;
    }

    public OasisMessage Build()
    {
        return new OasisMessage(From, Message, FromMessageId);
    }
}