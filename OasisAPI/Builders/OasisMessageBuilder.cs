using OasisAPI.Models;

namespace OasisAPI.Builders;

public class OasisMessageBuilder : IMyBuilder<OasisMessage>
{
    private string From;
    private string Message;
    private string? FromMessageId;
    private string? FromThreadId;

    public OasisMessageBuilder SetFrom(string from)
    {
        From = from;
        return this;
    }

    public OasisMessageBuilder SetMessage(string message)
    {
        Message = message;
        return this;
    }

    public OasisMessageBuilder SetFromMessageId(string? messageId)
    {
        FromMessageId = messageId;
        return this;
    }

    public OasisMessageBuilder SetFromThreadId(string? fromThreadId)
    {
        FromThreadId = fromThreadId;
        return this;
    }

    public OasisMessage Build()
    {
        return new OasisMessage(
            from: From,
            message: Message,
            fromThreadId: FromThreadId,
            fromMessageId: FromMessageId
        );
    }
}