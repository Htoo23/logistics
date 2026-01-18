using SharedKernel;

namespace Notifications.Domain;

public sealed class NotificationRequest : Entity
{
    private NotificationRequest() {}

    public string Channel { get; private set; } = default!; // sms,email,push
    public string Recipient { get; private set; } = default!;
    public string TemplateKey { get; private set; } = default!;
    public string PayloadJson { get; private set; } = default!;
    public DateTimeOffset ScheduledAt { get; private set; }
    public bool Sent { get; private set; }

    public static NotificationRequest Create(string channel, string recipient, string templateKey, string payloadJson, DateTimeOffset scheduledAt)
        => new() { Channel = channel, Recipient = recipient, TemplateKey = templateKey, PayloadJson = payloadJson, ScheduledAt = scheduledAt };

    public void MarkSent() => Sent = true;
}
