using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.RefundRequests.AcceptRefundRequest;

public sealed record AcceptRefundRequestCommand(Guid Id) : ICommand;