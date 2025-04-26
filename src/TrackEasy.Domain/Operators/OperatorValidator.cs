using FluentValidation;

namespace TrackEasy.Domain.Operators;

public sealed class OperatorValidator : AbstractValidator<Operator>
{
    public OperatorValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(3);
    }
}