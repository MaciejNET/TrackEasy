using FluentValidation;
using Shouldly;
using TrackEasy.Domain.Coaches;

namespace TrackEasy.UnitTests.Coaches;

public class SeatTests
{
    [Fact]
    public void Create_Should_CreateSeat_For_ValidNumber()
    {
        // Arrange
        const int validSeatNumber = 10;

        // Act
        var seat = Seat.Create(validSeatNumber);

        // Assert
        seat.ShouldNotBeNull();
        seat.Number.ShouldBe(validSeatNumber);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Create_Should_ThrowValidationException_For_InvalidNumber(int invalidSeatNumber)
    {
        // Act & Assert
        Should.Throw<ValidationException>(() => Seat.Create(invalidSeatNumber))
            .Message.ShouldContain("Seat number must be greater than 0.");
    }
}