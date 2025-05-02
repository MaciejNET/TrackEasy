using FluentValidation;

namespace TrackEasy.Domain.Trains;

internal sealed class TrainValidator : AbstractValidator<Train>
{
    public TrainValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(15);

        RuleFor(x => x.Coaches)
            .Must(x => x.Count == x.DistinctBy(c => c.Number).Count() &&
                       x.Count == x.DistinctBy(c => c.Id).Count())
            .WithMessage("Coaches must have unique numbers and IDs.");
        
        RuleFor(x => x.Coaches)
            .Must(x => x.All(c => c.Number > 0))
            .WithMessage("All coach numbers must be greater than 0.");
        
        RuleFor(x => x.Coaches)
            .NotEmpty()
            .Must(x => x.Count > 0)
            .WithMessage("Train must have at least one coach.");
    }
}