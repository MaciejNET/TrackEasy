using FluentValidation;

namespace TrackEasy.Domain.Trains;

internal sealed class TrainCoachValidator : AbstractValidator<TrainCoach>
{
    public TrainCoachValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Coach)
            .NotEmpty();
        
        RuleFor(x => x.Number)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Coach number must be greater than 0.");
    }
}