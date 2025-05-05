using FluentValidation;
using Shouldly;
using TrackEasy.Domain.Coaches;
using TrackEasy.Domain.Trains;

namespace TrackEasy.UnitTests.Trains;

public class TrainCoachTests
{
    [Fact]
    public void CreateTrainCoach_WithValidData_ShouldSucceed()
    {
        var coach = Coach.Create("COACH1", new List<Seat> { Seat.Create(1) }, Guid.NewGuid());
        const int validNumber = 1;
        
        var trainCoach = TrainCoach.Create(coach, validNumber);
        
        trainCoach.Id.ShouldNotBe(Guid.Empty);
        trainCoach.Coach.ShouldBe(coach);
        trainCoach.Number.ShouldBe(validNumber);
    }

    [Fact]
    public void CreateTrainCoach_WithInvalidNumber_ShouldThrowValidationException()
    {
        var coach = Coach.Create("COACH1", new List<Seat> { Seat.Create(1) }, Guid.NewGuid());
        const int invalidNumber = 0;
        
        Should.Throw<ValidationException>(() => TrainCoach.Create(coach, invalidNumber));
    }

    [Fact]
    public void UpdateTrainCoach_WithValidNumber_ShouldSucceed()
    {
        var coach = Coach.Create("COACH1", new List<Seat> { Seat.Create(1) }, Guid.NewGuid());
        var trainCoach = TrainCoach.Create(coach, 1);
        const int newValidNumber = 2;
        
        trainCoach.Update(newValidNumber);
        trainCoach.Number.ShouldBe(newValidNumber);
    }

    [Fact]
    public void UpdateTrainCoach_WithInvalidNumber_ShouldThrowValidationException()
    {
        var coach = Coach.Create("COACH1", new List<Seat> { Seat.Create(1) }, Guid.NewGuid());
        var trainCoach = TrainCoach.Create(coach, 1);
        
        Should.Throw<ValidationException>(() => trainCoach.Update(0));
    }
}