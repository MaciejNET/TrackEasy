using FluentValidation;

namespace TrackEasy.Domain.Connections;

internal sealed class ScheduleValidator : AbstractValidator<Schedule>
{
    public ScheduleValidator()
    {
        RuleFor(x => x)
            .Must(x => x.ValidFrom <= x.ValidTo);

        RuleFor(x => x.DaysOfWeek)
            .NotEmpty();
    }
}