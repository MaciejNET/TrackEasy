using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.RefundRequests.FindRefundRequest;

public sealed record FindRefundRequestQuery(Guid Id) : IQuery<RefundRequestDetailsDto?>;
