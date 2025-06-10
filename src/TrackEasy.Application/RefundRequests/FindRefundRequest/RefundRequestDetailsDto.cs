using TrackEasy.Application.Tickets.FindTicket;

namespace TrackEasy.Application.RefundRequests.FindRefundRequest;

public sealed record RefundRequestDetailsDto(
    Guid Id,
    int TicketNumber,
    IEnumerable<PersonDetailsDto> People,
    IEnumerable<int>? SeatNumbers,
    DateOnly ConnectionDate,
    IEnumerable<TicketConnectionStationDto> Stations,
    string OperatorCode,
    string OperatorName,
    string TrainName,
    Guid? QrCodeId,
    string Status,
    string Reason,
    DateTime CreatedAt)
{
    public TimeOnly DepartureTime => Stations.Single(x => x.SequenceNumber == 1).DepartureTime!.Value;
    public TimeOnly ArrivalTime => Stations.MaxBy(x => x.SequenceNumber)!.ArrivalTime!.Value;
}
