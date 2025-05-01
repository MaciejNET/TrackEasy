using FluentValidation;
using TrackEasy.Domain.Coaches;

namespace TrackEasy.Domain.Trains;

public sealed class TrainCoach
{
    public Guid Id { get; private set; }
    public Coach Coach { get; private set; }
    public int Number { get; private set; }

    internal static TrainCoach Create(Coach coach, int number)
    {
        var trainCoach = new TrainCoach
        {
            Id = Guid.NewGuid(),
            Coach = coach,
            Number = number
        };
        
        new TrainCoachValidator().ValidateAndThrow(trainCoach);
        return trainCoach;
    }

    internal void Update(int number)
    {
        Number = number;
        new TrainCoachValidator().ValidateAndThrow(this);
    }
    
#pragma warning disable CS8618
    private TrainCoach() { }
#pragma warning restore CS8618
}