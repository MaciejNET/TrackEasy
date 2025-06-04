using FluentValidation;
using TrackEasy.Domain.Stations;

namespace TrackEasy.Domain.Connections;

public sealed class ConnectionStation
{
    public Guid Id { get; private set; }
    public Guid StationId { get; private set; }
    public Station Station { get; private set; }
    public TimeOnly? ArrivalTime { get; private set; }
    public TimeOnly? DepartureTime { get; private set; }
    public int SequenceNumber { get; private set; }

    public static ConnectionStation Create(Station station, TimeOnly? arrivalDate, TimeOnly? departureDate,
        int sequenceNumber)
    {
        var connectionStation = new ConnectionStation
        {
            Id = Guid.NewGuid(),
            StationId = station.Id,
            Station = station,
            ArrivalTime = arrivalDate,
            DepartureTime = departureDate,
            SequenceNumber = sequenceNumber
        };
        
        new ConnectionStationValidator().ValidateAndThrow(connectionStation);
        return connectionStation;
    }
    
#pragma warning disable CS8618
    private ConnectionStation() {}
#pragma warning restore CS8618
}