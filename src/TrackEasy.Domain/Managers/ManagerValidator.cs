using FluentValidation;

namespace TrackEasy.Domain.Managers;

internal sealed class ManagerValidator : AbstractValidator<Manager>
{
    public ManagerValidator()
    {
        RuleFor(x => x.User)
            .NotNull();

        RuleFor(x => x.Operator)
            .NotNull();
    }
}