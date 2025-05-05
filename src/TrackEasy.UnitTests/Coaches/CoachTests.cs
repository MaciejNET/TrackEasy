using FluentValidation;
using Shouldly;
using TrackEasy.Domain.Coaches;

namespace TrackEasy.UnitTests.Coaches;

public class CoachTests
{
    private Seat CreateSeat(int number) => Seat.Create(number);

    private List<Seat> CreateValidSeats(int count = 5) =>
        Enumerable.Range(1, count).Select(CreateSeat).ToList();

    private Guid CreateValidOperatorId() => Guid.NewGuid();

    [Fact]
    public void Create_Should_CreateCoach_For_ValidData()
    {
        // Arrange
        var validCode = "ABC-123";
        var validSeats = CreateValidSeats();
        var validOperatorId = CreateValidOperatorId();

        // Act
        var coach = Coach.Create(validCode, validSeats, validOperatorId);

        // Assert
        coach.ShouldNotBeNull();
        coach.Id.ShouldNotBe(Guid.Empty);
        coach.Code.ShouldBe(validCode);
        coach.OperatorId.ShouldBe(validOperatorId);
        coach.Seats.Count.ShouldBe(validSeats.Count);
        coach.Seats.ShouldBe(validSeats);
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")] // Too short
    [InlineData("ABCDEFGHIJKLMNOP")] // Too long
    [InlineData(null)]
    public void Create_Should_ThrowValidationException_For_InvalidCode(string invalidCode)
    {
        // Arrange
        var validSeats = CreateValidSeats();
        var validOperatorId = CreateValidOperatorId();

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            Coach.Create(invalidCode, validSeats, validOperatorId));
    }

    [Fact]
    public void Create_Should_ThrowValidationException_For_EmptySeats()
    {
        // Arrange
        var validCode = "XYZ-789";
        var emptySeats = new List<Seat>();
        var validOperatorId = CreateValidOperatorId();

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            Coach.Create(validCode, emptySeats, validOperatorId))
            .Message.ShouldContain("At least one seat is required.");
    }

    [Fact]
    public void Create_Should_ThrowValidationException_For_DuplicateSeats()
    {
        // Arrange
        var validCode = "DUP-001";
        var duplicateSeats = new List<Seat> { CreateSeat(1), CreateSeat(2), CreateSeat(1) };
        var validOperatorId = CreateValidOperatorId();

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            Coach.Create(validCode, duplicateSeats, validOperatorId))
            .Message.ShouldContain("Seats must be unique.");
    }

    [Fact]
    public void Update_Should_UpdateCodeAndSeats_For_ValidData()
    {
        // Arrange
        var coach = Coach.Create("OLD-CODE", CreateValidSeats(3), CreateValidOperatorId());
        var newCode = "NEW-CODE";
        var newSeats = CreateValidSeats(2);

        // Act
        coach.Update(newCode, newSeats);

        // Assert
        coach.Code.ShouldBe(newCode);
        coach.Seats.Count.ShouldBe(newSeats.Count);
        coach.Seats.ShouldBe(newSeats);
    }

    [Theory]
    [InlineData("")]
    [InlineData("XY")] // Too short
    [InlineData("1234567890123456")] // Too long
    [InlineData(null)]
    public void Update_Should_ThrowValidationException_For_InvalidCode(string invalidCode)
    {
        // Arrange
        var coach = Coach.Create("VALID-CODE", CreateValidSeats(), CreateValidOperatorId());
        var validSeats = CreateValidSeats(2);

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            coach.Update(invalidCode, validSeats));
    }

    [Fact]
    public void Update_Should_ThrowValidationException_For_EmptySeats()
    {
        // Arrange
        var coach = Coach.Create("VALID-CODE", CreateValidSeats(), CreateValidOperatorId());
        var emptySeats = new List<Seat>();

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            coach.Update("NEW-CODE", emptySeats))
            .Message.ShouldContain("At least one seat is required.");
    }

    [Fact]
    public void Update_Should_ThrowValidationException_For_DuplicateSeats()
    {
        // Arrange
        var coach = Coach.Create("VALID-CODE", CreateValidSeats(), CreateValidOperatorId());
        var duplicateSeats = new List<Seat> { CreateSeat(5), CreateSeat(6), CreateSeat(5) };

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            coach.Update("NEW-CODE", duplicateSeats))
            .Message.ShouldContain("Seats must be unique.");
    }
}