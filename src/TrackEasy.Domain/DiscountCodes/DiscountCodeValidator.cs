using FluentValidation;

namespace TrackEasy.Domain.DiscountCodes;

public sealed class DiscountCodeValidator : AbstractValidator<DiscountCode>
{
    public DiscountCodeValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(15)
            .Matches("^[a-zA-Z0-9-_]{5,15}$");
        
        RuleFor(x => x.Percentage)
            .ExclusiveBetween(0, 100);
        
        RuleFor(x => x.From)
            .NotEmpty()
            .LessThan(x => x.To)
            .GreaterThanOrEqualTo(timeProvider.GetUtcNow().DateTime);

        RuleFor(x => x.To)
            .NotEmpty()
            .GreaterThan(x => x.From);
    }
}