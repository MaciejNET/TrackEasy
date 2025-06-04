namespace TrackEasy.Pdf.Abstractions.Models;

public sealed record TicketModel(
    Guid Id,
    int TicketNumber,
    string ConnectionName,
    string TrainName,
    string OperatorName,
    DateOnly ConnectionDate,
    string StartStation,
    TimeOnly DepartureDate,
    string EndStation,
    TimeOnly ArrivalDate,
    IEnumerable<PassengerModel> Passengers,
    IEnumerable<int>? SeatNumbers,
    decimal Price,
    string Currency,
    string TicketStatus,
    byte[] QrCode
);

public sealed record PassengerModel(string FirstName, string LastName);