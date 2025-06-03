using FluentValidation;
using TrackEasy.Domain.Coaches;
using TrackEasy.Shared.Domain.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Domain.Trains;

public sealed class Train : AggregateRoot
{
    private readonly List<TrainCoach> _coaches = [];
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid OperatorId { get; private set; }
    public IReadOnlyList<TrainCoach> Coaches => _coaches.AsReadOnly();

    internal static Train Create(string name, IEnumerable<(Coach Coach, int Number)> coaches, Guid operatorId)
    {
        var train = new Train
        {
            Id = Guid.NewGuid(),
            Name = name,
            OperatorId = operatorId
        };
        
        foreach (var (coach, number) in coaches)
        {
            train.AddCoach(coach, number);
        }
        
        new TrainValidator().ValidateAndThrow(train);
        return train;
    }

    internal void Update(string name, IEnumerable<(Coach Coach, int Number)> coaches)
    {
        _coaches.Clear();
        foreach (var (coach, number) in coaches)
        {
            AddCoach(coach, number);
        }
        
        Name = name;
        new TrainValidator().ValidateAndThrow(this);
    }
    
    
    private void AddCoach(Coach coach, int number)
    {
        if (_coaches.Any(tc => tc.Coach.Id == coach.Id))
        {
            throw new TrackEasyException(Codes.CoachAlreadyAdded, $"Coach with ID {coach.Id} is already added to the train.");
        }
        if (_coaches.Any(tc => tc.Number == number))
        {
            throw new TrackEasyException(Codes.CoachNumberAlreadyExists, $"A coach with number {number} already exists in the train.");
        }
        var trainCoach = TrainCoach.Create(coach, number);
        _coaches.Add(trainCoach);
        new TrainValidator().ValidateAndThrow(this);
    }
    
#pragma warning disable CS8618
    private Train() { }
#pragma warning restore CS8618
}