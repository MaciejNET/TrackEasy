using FluentValidation;
using TrackEasy.Domain.Shared;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.DiscountCodes;

public sealed class DiscountCode : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Code { get; private set; }
    public int Percentage { get; private set; }
    public DateTime From { get; private set; }
    public DateTime To { get; private set; }

    public static DiscountCode Create(string code, int percentage, DateTime from, DateTime to, TimeProvider timeProvider)
    {
        var discountCode = new DiscountCode
        {
            Id = Guid.NewGuid(),
            Code = code,
            Percentage = percentage,
            From = from,
            To = to
        };
        
        new DiscountCodeValidator(timeProvider).ValidateAndThrow(discountCode);
        return discountCode;
    }
    
    public void Update(int percentage, DateTime from, DateTime to, TimeProvider timeProvider)
    {
        Percentage = percentage;
        From = from;
        To = to;
        new DiscountCodeValidator(timeProvider).ValidateAndThrow(this);
    }

    public bool IsActive(TimeProvider timeProvider)
    {
        return timeProvider.GetUtcNow().DateTime >= From && timeProvider.GetUtcNow().DateTime <= To;
    }

    public Money CalculateDiscount(Money price)
    {
        var discountAmount = price.Amount * (Percentage / 100m);
        return new Money(price.Amount - discountAmount, price.Currency);
    }
    
#pragma warning disable CS8618, CS9264
    private DiscountCode() {}
#pragma warning restore CS8618, CS9264
}