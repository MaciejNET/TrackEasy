using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Notifications;

public sealed class Notification : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Message { get; private set; }
    public Guid UserId { get; private set; }
    public NotificationType Type { get; private set; }
    public Guid ObjectId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public static Notification Create(string title, string message, Guid userId,
        NotificationType type, Guid objectId, TimeProvider timeProvider)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Title = title,
            Message = message,
            UserId = userId,
            Type = type,
            ObjectId = objectId,
            CreatedAt = timeProvider.GetUtcNow().DateTime
        };
        
        return notification;
    }
    
#pragma warning disable CS8618
    private Notification() { }
#pragma warning restore CS8618
}