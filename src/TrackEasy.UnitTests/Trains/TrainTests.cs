using FluentValidation;
using Shouldly;
using TrackEasy.Domain.Coaches;
using TrackEasy.Domain.Trains;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.UnitTests.Trains;

public class TrainTests
{
    private static Coach CreateCoach(string code, int seatCount = 1, Guid? operatorId = null)
    {
        var seats = Enumerable.Range(1, seatCount)
            .Select(seatNumber => Seat.Create(seatNumber))
            .ToList();
        
        return Coach.Create(code, seats, operatorId ?? Guid.NewGuid());
    }

    [Fact]
    public void Create_WithValidParameters_ShouldSucceed()
    {
        // Arrange
        const string name = "Express Train";
        var operatorId = Guid.NewGuid();
        var coach1 = CreateCoach("COACH1", 2, operatorId);
        var coach2 = CreateCoach("COACH2", 2, operatorId);
        var coaches = new List<(Coach Coach, int Number)>
        {
            (coach1, 1),
            (coach2, 2)
        };

        // Act
        var train = Train.Create(name, coaches, operatorId);

        // Assert
        train.ShouldNotBeNull();
        train.Id.ShouldNotBe(Guid.Empty);
        train.Name.ShouldBe(name);
        train.OperatorId.ShouldBe(operatorId);
        train.Coaches.Count.ShouldBe(2);
        train.Coaches[0].Coach.ShouldBeSameAs(coach1);
        train.Coaches[0].Number.ShouldBe(1);
        train.Coaches[1].Coach.ShouldBeSameAs(coach2);
        train.Coaches[1].Number.ShouldBe(2);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowValidationException()
    {
        // Arrange
        var emptyName = string.Empty;
        var operatorId = Guid.NewGuid();
        var coach = CreateCoach("COACH1", 2, operatorId);
        var coaches = new List<(Coach Coach, int Number)> { (coach, 1) };

        // Act & Assert
        Should.Throw<ValidationException>(() => 
            Train.Create(emptyName, coaches, operatorId));
    }

    [Fact]
    public void Create_WithDuplicateCoachNumbers_ShouldThrowException()
    {
        // Arrange
        const string name = "Express Train";
        var operatorId = Guid.NewGuid();
        var coach1 = CreateCoach("COACH1", 2, operatorId);
        var coach2 = CreateCoach("COACH2", 2, operatorId);
        var coaches = new List<(Coach Coach, int Number)>
        {
            (coach1, 1),
            (coach2, 1)
        };

        // Act & Assert
        Should.Throw<TrackEasyException>(() => 
            Train.Create(name, coaches, operatorId))
            .Code.ShouldBe(Codes.CoachNumberAlreadyExists);
    }

    [Fact]
    public void Create_WithDuplicateCoaches_ShouldThrowException()
    {
        // Arrange
        const string name = "Express Train";
        var operatorId = Guid.NewGuid();
        var coach1 = CreateCoach("COACH1", 2, operatorId);
        var coaches = new List<(Coach Coach, int Number)>
        {
            (coach1, 1),
            (coach1, 2)
        };

        // Act & Assert
        Should.Throw<TrackEasyException>(() => 
            Train.Create(name, coaches, operatorId))
            .Code.ShouldBe(Codes.CoachAlreadyAdded);
    }

    [Fact]
    public void Update_WithValidParameters_ShouldSucceed()
    {
        // Arrange
        const string originalName = "Original Train";
        const string updatedName = "Updated Train";
        var operatorId = Guid.NewGuid();
        
        var coach1 = CreateCoach("COACH1", 2, operatorId);
        var coach2 = CreateCoach("COACH2", 2, operatorId);
        var coach3 = CreateCoach("COACH3", 2, operatorId);
        
        var originalCoaches = new List<(Coach Coach, int Number)>
        {
            (coach1, 1),
            (coach2, 2)
        };
        
        var updatedCoaches = new List<(Coach Coach, int Number)>
        {
            (coach2, 1),
            (coach3, 2)
        };

        var train = Train.Create(originalName, originalCoaches, operatorId);

        // Act
        train.Update(updatedName, updatedCoaches);

        // Assert
        train.Name.ShouldBe(updatedName);
        train.Coaches.Count.ShouldBe(2);
        train.Coaches[0].Coach.ShouldBeSameAs(coach2);
        train.Coaches[0].Number.ShouldBe(1);
        train.Coaches[1].Coach.ShouldBeSameAs(coach3);
        train.Coaches[1].Number.ShouldBe(2);
    }

    [Fact]
    public void Update_WithEmptyName_ShouldThrowValidationException()
    {
        // Arrange
        const string originalName = "Original Train";
        var emptyName = string.Empty;
        var operatorId = Guid.NewGuid();
        
        var coach = CreateCoach("COACH1", 2, operatorId);
        var coaches = new List<(Coach Coach, int Number)> { (coach, 1) };
        
        var train = Train.Create(originalName, coaches, operatorId);

        // Act & Assert
        Should.Throw<ValidationException>(() => 
            train.Update(emptyName, coaches));
    }
    
    [Fact]
    public void Update_WithDuplicateCoachNumbers_ShouldThrowException()
    {
        // Arrange
        const string name = "Express Train";
        var operatorId = Guid.NewGuid();
        
        var coach = CreateCoach("COACH1", 2, operatorId);
        var originalCoaches = new List<(Coach Coach, int Number)> { (coach, 1) };
        
        var coach1 = CreateCoach("COACH2", 2, operatorId);
        var coach2 = CreateCoach("COACH3", 2, operatorId);
        var updatedCoaches = new List<(Coach Coach, int Number)>
        {
            (coach1, 1),
            (coach2, 1) 
        };
        
        var train = Train.Create(name, originalCoaches, operatorId);

        // Act & Assert
        Should.Throw<TrackEasyException>(() => 
            train.Update(name, updatedCoaches))
            .Code.ShouldBe(Codes.CoachNumberAlreadyExists);
    }

    [Fact]
    public void Update_WithDuplicateCoaches_ShouldThrowException()
    {
        // Arrange
        const string name = "Express Train";
        var operatorId = Guid.NewGuid();
        
        var coach = CreateCoach("COACH1", 2, operatorId);
        var originalCoaches = new List<(Coach Coach, int Number)> { (coach, 1) };
        
        var newCoach = CreateCoach("COACH2", 2, operatorId);
        var updatedCoaches = new List<(Coach Coach, int Number)>
        {
            (newCoach, 1),
            (newCoach, 2)
        };
        
        var train = Train.Create(name, originalCoaches, operatorId);

        // Act & Assert
        Should.Throw<TrackEasyException>(() => 
            train.Update(name, updatedCoaches))
            .Code.ShouldBe(Codes.CoachAlreadyAdded);
    }
}