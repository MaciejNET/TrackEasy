using FluentValidation;

namespace TrackEasy.Domain.Coaches;

internal sealed class CoachValidator : AbstractValidator<Coach>
{
    public CoachValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(15);

        RuleFor(x => x.Seats)
            .NotEmpty();
    }
}