using FluentValidation;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Coaches;

public sealed class Coach : AggregateRoot
{
    private List<Seat> _seats = [];
    
    public Guid Id { get; private set; }
    public string Code { get; private set; }
    public Guid OperatorId { get; private set; }
    public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();
    
    internal static Coach Create(string code, IEnumerable<Seat> seats, Guid operatorId)
    {
        var coach = new Coach
        {
            Id = Guid.NewGuid(),
            Code = code,
            OperatorId = operatorId,
            _seats = [..seats]
        };

        new CoachValidator().ValidateAndThrow(coach);
        return coach;
    }
    
    internal void Update(string code, IEnumerable<Seat> seats)
    {
        Code = code;
        _seats = [..seats];

        new CoachValidator().ValidateAndThrow(this);
    }
    
#pragma warning disable CS8618, CS9264
    private Coach() { }
#pragma warning restore CS8618, CS9264
}