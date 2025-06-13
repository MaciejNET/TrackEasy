using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Notifications.DeleteNotification;

public sealed record DeleteNotificationCommand(Guid Id) : ICommand;
