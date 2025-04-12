using FluentValidation;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Discounts;

public sealed class Discount : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int Percentage { get; private set; }
    public static Discount Create(string name, int percentage)
    {
        var discount = new Discount
        {
            Id = Guid.NewGuid(),
            Name = name,
            Percentage = percentage,
        };

        new DiscountValidator().ValidateAndThrow(discount);
        return discount;
    }

    public void Update(string name, int percentage)
    {
        Name = name;
        Percentage = percentage;

        new DiscountValidator().ValidateAndThrow(this);
    }
    
#pragma warning disable CS8618, CS9264
    private Discount() { }
#pragma warning restore CS8618, CS9264
}
