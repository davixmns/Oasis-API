using OasisAPI.Models;

namespace OasisAPI.Builders;

public class OasisMessageBuilder : IMyBuilder<OasisMessage>
{
    private string Name;
    private string Message;
    private string? ThreadId;
    private string? MessageId;

    public OasisMessageBuilder SetName(string name)
    {
        Name = name;
        return this;
    }

    public OasisMessageBuilder SetMessage(string message)
    {
        Message = message;
        return this;
    }

    public OasisMessageBuilder SetThreadId(string? threadId)
    {
        this.ThreadId = threadId;
        return this;
    }

    public OasisMessageBuilder SetMessageId(string? messageId)
    {
        this.MessageId = messageId;
        return this;
    }

    public OasisMessage Build()
    {
        return new OasisMessage(Name, Message, ThreadId, MessageId);
    }
}