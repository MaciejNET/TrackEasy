using FluentValidation;

namespace TrackEasy.Domain.Stations;

internal sealed class StationValidator : AbstractValidator<Station>
{
    public StationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MinimumLength(50);

        RuleFor(x => x.CityId)
            .NotEmpty();

        RuleFor(x => x.City)
            .NotEmpty();
    }
}