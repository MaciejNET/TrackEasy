using FluentValidation;

namespace TrackEasy.Domain.Connections;

public sealed record ConnectionRequest
{
    public ConnectionRequestType RequestType { get; private set; }
    public Guid? Id { get; private set; }
    public Schedule? Schedule { get; private set; }
    public IReadOnlyList<ConnectionStation>? Stations { get; private set; }

    public static ConnectionRequest CreateAddRequest()
        => new(ConnectionRequestType.ADD, null, null, null);
    
    public static ConnectionRequest CreateUpdateRequest(Guid id, Schedule schedule, IEnumerable<ConnectionStation> stations)
        => new(ConnectionRequestType.UPDATE, id, schedule, stations);
    
    public static ConnectionRequest CreateDeleteRequest()
        => new(ConnectionRequestType.DELETE, null, null, null);
    
    private ConnectionRequest(ConnectionRequestType requestType, Guid? id, Schedule? schedule, IEnumerable<ConnectionStation>? stations)
    {
        RequestType = requestType;
        Id = id;
        Schedule = schedule;
        Stations = stations?.ToList();
        
        new ConnectionRequestValidator().ValidateAndThrow(this);
    }
}