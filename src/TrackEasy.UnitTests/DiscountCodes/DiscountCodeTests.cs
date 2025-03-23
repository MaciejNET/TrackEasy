using Microsoft.Extensions.Time.Testing;
using Shouldly;
using TrackEasy.Domain.DiscountCodes;
using ValidationException = FluentValidation.ValidationException;

namespace TrackEasy.UnitTests.DiscountCodes;

public class DiscountCodeTests
{
    [Theory]
    [InlineData("DISCOUNT10")]
    [InlineData("SUMMER20")]
    [InlineData("WINTER30")]
    [InlineData("SPRING40")]
    [InlineData("FALL-50")]
    [InlineData("NEW_YEAR_60")]
    [InlineData("black-firday-70")]
    [InlineData("dc-80")]
    public void CreateDiscountCode_ValidCode_ShouldCreateDiscountCode(string code)
    {
        // Arrange
        const int percentage = 10;
        var from = new DateTime(2025, 03, 01);
        var to = new DateTime(2025, 04, 01);
        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2025, 02, 26, 0, 0, 0, TimeSpan.Zero));

        // Act
        var discountCode = DiscountCode.Create(code, percentage, from, to, timeProvider);

        // Assert
        discountCode.ShouldNotBeNull();
        discountCode.Code.ShouldBe(code);
        discountCode.Percentage.ShouldBe(percentage);
        discountCode.From.ShouldBe(from);
        discountCode.To.ShouldBe(to);
        discountCode.Id.ShouldNotBe(Guid.Empty);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(99)]
    public void CreateDiscountCode_ValidPercentage_ShouldCreateDiscountCode(int percentage)
    {
        // Arrange
        const string code = "DISCOUNT10";
        var from = new DateTime(2025, 03, 01);
        var to = new DateTime(2025, 04, 01);
        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2025, 02, 26, 0, 0, 0, TimeSpan.Zero));

        // Act
        var discountCode = DiscountCode.Create(code, percentage, from, to, timeProvider);

        // Assert
        discountCode.ShouldNotBeNull();
        discountCode.Percentage.ShouldBe(percentage);
    }
    
    [Theory]
    [InlineData("2025-03-01", "2025-04-01")]
    [InlineData("2025-03-15", "2025-04-15")]
    [InlineData("2025-03-01", "2025-03-31")]
    public void CreateDiscountCode_ValidDates_ShouldCreateDiscountCode(string fromString, string toString)
    {
        // Arrange
        var from = DateTime.Parse(fromString);
        var to = DateTime.Parse(toString);
        const string code = "DISCOUNT10";
        const int percentage = 10;
        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2025, 02, 26, 0, 0, 0, TimeSpan.Zero));

        // Act
        var discountCode = DiscountCode.Create(code, percentage, from, to, timeProvider);

        // Assert
        discountCode.ShouldNotBeNull();
        discountCode.From.ShouldBe(from);
        discountCode.To.ShouldBe(to);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("DISCOUNT10DISCOUNT10")]
    [InlineData("DISCOUNT10!")]
    [InlineData("DISCOUNT10@")]
    [InlineData("DISCOUNT10#")]
    [InlineData("DISCOUNT10$")]
    [InlineData("ąćęłńóśźż")]
    public void CreateDiscountCode_InvalidCode_ShouldThrowValidationException(string code)
    {
        // Arrange
        const int percentage = 10;
        var from = new DateTime(2025, 03, 01);
        var to = new DateTime(2025, 04, 01);
        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2025, 02, 26, 0, 0, 0, TimeSpan.Zero));

        // Act & Assert
        Should.Throw<FluentValidation.ValidationException>(() => DiscountCode.Create(code, percentage, from, to, timeProvider));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(101)]
    [InlineData(-1)]
    public void CreateDiscountCode_InvalidPercentage_ShouldThrowValidationException(int percentage)
    {
        // Arrange
        const string code = "DISCOUNT10";
        var from = new DateTime(2025, 03, 01);
        var to = new DateTime(2025, 04, 01);
        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2025, 02, 26, 0, 0, 0, TimeSpan.Zero));
        
        // Act
        var act = () => DiscountCode.Create(code, percentage, from, to, timeProvider);
        
        // Assert
        Should.Throw<ValidationException>(act);
    }

    [Theory]
    [InlineData("2025-03-01", "2025-02-28")]
    [InlineData("2025-03-01", "2025-03-01")]
    [InlineData("2025-02-25", "2025-03-01")]
    public void CreateDiscountCode_InvalidFrom_ShouldThrowValidationException(string fromString, string toString)
    {
        // Arrange
        var from = DateTime.Parse(fromString);
        var to = DateTime.Parse(toString);
        const string code = "DISCOUNT10";
        const int percentage = 10;
        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2025, 02, 26, 0, 0, 0, TimeSpan.Zero));
        
        // Act
        var act = () => DiscountCode.Create(code, percentage, from, to, timeProvider);
        
        // Assert
        Should.Throw<ValidationException>(act);
    }
}