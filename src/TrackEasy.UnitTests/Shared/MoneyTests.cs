using Shouldly;
using TrackEasy.Domain.Shared;

namespace TrackEasy.UnitTests.Shared;

public class MoneyTests
{
    [Fact]
    public void Constructor_WithValidAmount_Should_SetProperties()
    {
        var money = new Money(100, Currency.USD);
        money.Amount.ShouldBe(100);
        money.Currency.ShouldBe(Currency.USD);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Constructor_WithInvalidAmount_ShouldThrowValidationException(decimal invalidAmount)
    {
        Assert.Throws<FluentValidation.ValidationException>(() => new Money(invalidAmount, Currency.USD));
    }

    [Fact]
    public void Addition_WithSameCurrency_ShouldReturnCorrectResult()
    {
        var m1 = new Money(50, Currency.USD);
        var m2 = new Money(70, Currency.USD);
        var result = m1 + m2;
        result.Amount.ShouldBe(120);
        result.Currency.ShouldBe(Currency.USD);
    }

    [Fact]
    public void Addition_WithDifferentCurrencies_ShouldThrowInvalidOperationException()
    {
        var m1 = new Money(50, Currency.USD);
        var m2 = new Money(70, Currency.EUR);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var _ = m1 + m2;
        });
    }

    [Fact]
    public void Subtraction_WithSameCurrency_ShouldReturnCorrectResult()
    {
        var m1 = new Money(100, Currency.EUR);
        var m2 = new Money(40, Currency.EUR);
        var result = m1 - m2;
        result.Amount.ShouldBe(60);
        result.Currency.ShouldBe(Currency.EUR);
    }

    [Fact]
    public void Subtraction_WithDifferentCurrencies_ShouldThrowInvalidOperationException()
    {
        var m1 = new Money(100, Currency.EUR);
        var m2 = new Money(40, Currency.USD);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var _ = m1 - m2;
        });
    }
}