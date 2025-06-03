using FluentValidation;

namespace TrackEasy.Domain.Shared;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
public sealed record Money
{
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }
    
    public Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;

        new MoneyValidator().ValidateAndThrow(this);
    }
    
    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot add money with different currencies.");

        return new Money(left.Amount + right.Amount, left.Currency);
    }
    
    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot subtract money with different currencies.");

        return new Money(left.Amount - right.Amount, left.Currency);
    }

    public long ToMinorUnits()
    {
        return (long)(Amount * 100);
    }

    public string GetCurrencyCode()
    {
        return Currency switch
        {
            Currency.USD => "usd",
            Currency.EUR => "eur",
            Currency.PLN => "pln",
            _ => throw new NotSupportedException($"Currency {Currency} is not supported.")
        };
    }
}