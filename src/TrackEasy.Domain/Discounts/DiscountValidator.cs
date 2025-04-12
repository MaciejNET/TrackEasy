using FluentValidation;

namespace TrackEasy.Domain.Discounts;

public sealed class DiscountValidator : AbstractValidator<Discount>
{
    public DiscountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Percentage)
            .InclusiveBetween(1, 99);
    }
}
