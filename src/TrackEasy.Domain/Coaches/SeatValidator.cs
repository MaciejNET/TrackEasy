using FluentValidation;

namespace TrackEasy.Domain.Coaches;

internal sealed class SeatValidator : AbstractValidator<Seat>
{
    public SeatValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Seat number must be greater than 0.");
    }
}