using FluentValidation;

namespace TrackEasy.Domain.Shared;

internal sealed class MoneyValidator : AbstractValidator<Money>
{
    public MoneyValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}