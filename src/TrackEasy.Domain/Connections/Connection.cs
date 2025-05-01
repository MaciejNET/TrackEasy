using FluentValidation;
using TrackEasy.Domain.Operators;
using TrackEasy.Domain.Shared;
using TrackEasy.Shared.Domain.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Domain.Connections;

public sealed class Connection : AggregateRoot
{
    private List<ConnectionStation> _stations = [];
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Operator Operator { get; private set; }
    public Money PricePerKilometer { get; private set; }
    public Schedule Schedule { get; private set; }
    public IReadOnlyList<ConnectionStation> Stations => _stations.AsReadOnly();
    public ConnectionRequest? Request { get; private set; }
    public bool IsActivated { get; private set; }

    public static Connection Create(string name, Operator @operator, Money pricePerKilometer,
        Schedule schedule, IEnumerable<ConnectionStation> stations)
    {
        var connection = new Connection
        {
            Id = Guid.NewGuid(),
            Name = name,
            Operator = @operator,
            PricePerKilometer = pricePerKilometer,
            Schedule = schedule,
            _stations = [..stations],
            Request = ConnectionRequest.CreateAddRequest(),
            IsActivated = false
        };
        
        connection.AddDomainEvent(new ConnectionRequestCreatedEvent(connection.Id, connection.Name, connection.Operator.Name, connection.Request.RequestType));
        new ConnectionValidator().ValidateAndThrow(connection);
        return connection;
    }
    
    public void Update(string name,  Money pricePerKilometer)
    {
        Name = name;
        PricePerKilometer = pricePerKilometer;
        new ConnectionValidator().ValidateAndThrow(this);
    }
    
    public void UpdateSchedule(Schedule schedule, IEnumerable<ConnectionStation> stations)
    {
        Request = ConnectionRequest.CreateUpdateRequest(Id, schedule, stations);
        AddDomainEvent(new ConnectionRequestCreatedEvent(Id, Name, Operator.Name, Request.RequestType));
    }
    
    public void Delete()
    {
        Request = ConnectionRequest.CreateDeleteRequest();
        AddDomainEvent(new ConnectionRequestCreatedEvent(Id, Name, Operator.Name, Request.RequestType));
    }

    public void ApproveRequest()
    {
        if (Request is null)
        {
            throw new TrackEasyException(Codes.ConnectionRequestNotFound, "No request found to approve.");
        }
        
        switch (Request.RequestType)
        {
            case ConnectionRequestType.ADD:
                IsActivated = true;
                break;
            case ConnectionRequestType.UPDATE:
                Schedule = Request.Schedule!;
                _stations = [..Request.Stations!];
                new ConnectionValidator().ValidateAndThrow(this);
                break;
            case ConnectionRequestType.DELETE:
                AddDomainEvent(new ConnectionDeletedEvent(Id));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        AddDomainEvent(new ConnectionRequestApprovedEvent(Id, Name, Operator.Id, Request.RequestType));
        Request = null;
    }
    
    public void RejectRequest()
    {
        if (Request is null)
        {
            throw new TrackEasyException(Codes.ConnectionRequestNotFound, "No request found to reject.");
        }
        
        AddDomainEvent(new ConnectionRequestRejectedEvent(Id, Name, Operator.Id, Request.RequestType));
        if (Request.RequestType is ConnectionRequestType.ADD)
        {
            AddDomainEvent(new ConnectionDeletedEvent(Id));
        }
        Request = null;
    }
    
#pragma warning disable CS8618
    private Connection() { }
#pragma warning restore CS8618
}