using FluentValidation;
using TrackEasy.Domain.Stations;

namespace TrackEasy.Domain.Stations;

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
    }
}
