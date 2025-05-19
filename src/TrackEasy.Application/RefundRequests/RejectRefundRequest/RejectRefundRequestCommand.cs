using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.RefundRequests.RejectRefundRequest;

public sealed record RejectRefundRequestCommand(Guid Id) : ICommand;