using FluentValidation;
using Shouldly;
using TrackEasy.Domain.Discounts;

namespace TrackEasy.UnitTests.Discounts;

public class DiscountTests
{
    private static Discount CreateValidDiscount() =>
        Discount.Create("Student Discount", 15);

    [Fact]
    public void Create_Should_CreateDiscount_For_ValidData()
    {
        // Arrange
        const string validName = "Senior Discount";
        const int validPercentage = 10;

        // Act
        var discount = Discount.Create(validName, validPercentage);

        // Assert
        discount.ShouldNotBeNull();
        discount.Id.ShouldNotBe(Guid.Empty);
        discount.Name.ShouldBe(validName);
        discount.Percentage.ShouldBe(validPercentage);
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")] // Too short
    [InlineData("This name is definitely longer than fifty characters limit")] // Too long
    [InlineData(null)]
    public void Create_Should_ThrowValidationException_For_InvalidName(string invalidName)
    {
        // Arrange
        const int validPercentage = 20;

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            Discount.Create(invalidName, validPercentage));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(-5)]
    [InlineData(101)]
    public void Create_Should_ThrowValidationException_For_InvalidPercentage(int invalidPercentage)
    {
        // Arrange
        const string validName = "Valid Discount";

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            Discount.Create(validName, invalidPercentage));
    }

    [Fact]
    public void Update_Should_UpdateNameAndPercentage_For_ValidData()
    {
        // Arrange
        var discount = CreateValidDiscount();
        const string newName = "Updated Discount";
        const int newPercentage = 25;

        // Act
        discount.Update(newName, newPercentage);

        // Assert
        discount.Name.ShouldBe(newName);
        discount.Percentage.ShouldBe(newPercentage);
    }

    [Theory]
    [InlineData("")]
    [InlineData("XY")] // Too short
    [InlineData("This updated name is also way too long for the fifty character limit")] // Too long
    public void Update_Should_ThrowValidationException_For_InvalidName(string invalidName)
    {
        // Arrange
        var discount = CreateValidDiscount();
        const int validPercentage = 30;

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            discount.Update(invalidName, validPercentage));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(-10)]
    [InlineData(150)]
    public void Update_Should_ThrowValidationException_For_InvalidPercentage(int invalidPercentage)
    {
        // Arrange
        var discount = CreateValidDiscount();
        const string validName = "Still Valid";

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            discount.Update(validName, invalidPercentage));
    }
}