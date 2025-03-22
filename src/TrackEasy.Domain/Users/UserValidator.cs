using FluentValidation;

namespace TrackEasy.Domain.Users;

internal sealed class UserValidator : AbstractValidator<User>
{
    public UserValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);
        
        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .Must(x => BeAtLeast13YearsOld(x!.Value, timeProvider));
    }
    
    private static bool BeAtLeast13YearsOld(DateOnly dateOfBirth, TimeProvider timeProvider)
    {
        var today = DateOnly.FromDateTime(timeProvider.GetUtcNow().Date);
        var age = today.Year - dateOfBirth.Year;
        
        if (dateOfBirth > today.AddYears(-age))
        {
            age--;
        }
        
        return age >= 13;
    }
}