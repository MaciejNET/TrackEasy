using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.RefundRequests.CreateRefundRequest;

public sealed record CreateRefundRequestCommand(Guid? UserId, Guid TicketId, string Reason) : ICommand<Guid>;