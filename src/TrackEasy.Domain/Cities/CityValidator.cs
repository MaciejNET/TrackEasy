using FluentValidation;

namespace TrackEasy.Domain.Cities;

public sealed class CityValidator : AbstractValidator<City>
{
    public CityValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Country)
            .IsInEnum();

        RuleForEach(x => x.FunFacts)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);
    }
}
