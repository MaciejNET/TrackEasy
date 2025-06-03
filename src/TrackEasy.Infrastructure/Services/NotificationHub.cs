using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TrackEasy.Infrastructure.Services;

[Authorize]
public sealed class NotificationHub : Hub<INotificationClient>;