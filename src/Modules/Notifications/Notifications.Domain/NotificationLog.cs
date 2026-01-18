using SharedKernel;

namespace Notifications.Domain;

public sealed class NotificationLog : Entity
{
    private NotificationLog() {}

    public string Channel { get; private set; } = default!; // SMS/Email/Push
    public string Recipient { get; private set; } = default!;
    public string TemplateKey { get; private set; } = default!;
    public string PayloadJson { get; private set; } = default!;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? SentAt { get; private set; }
    public string Status { get; private set; } = "Queued";

    public static NotificationLog Queue(string channel, string recipient, string templateKey, string payloadJson)
        => new() { Channel = channel, Recipient = recipient, TemplateKey = templateKey, PayloadJson = payloadJson, CreatedAt = DateTimeOffset.UtcNow, Status = "Queued" };

    public void MarkSent()
    {
        SentAt = DateTimeOffset.UtcNow;
        Status = "Sent";
    }
}
