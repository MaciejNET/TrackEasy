namespace TrackEasy.Application.RefundRequests.GetRefundRequests;

public sealed record RefundRequestDto(
    Guid Id,
    Guid TicketId,
    int TicketNumber,
    string EmailAddress,
    DateOnly ConnectionDate,
    string Reason,
    DateTime CreatedAt);
