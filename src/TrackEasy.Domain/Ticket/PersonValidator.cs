using FluentValidation;

namespace TrackEasy.Domain.Ticket;

internal sealed class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty();

        When(x => x.Discount is not null, () =>
        {
            RuleFor(x => x.Discount)
                .MinimumLength(3)
                .MaximumLength(50);
        });
    }
}