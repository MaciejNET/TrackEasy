using FluentValidation;

namespace TrackEasy.Domain.Connections;

public sealed record Schedule
{
    private readonly List<DayOfWeek> _daysOfWeek = [];
    
    public DateOnly ValidFrom { get; private set; }
    public DateOnly ValidTo { get; private set; }
    public IReadOnlyCollection<DayOfWeek> DaysOfWeek => _daysOfWeek.AsReadOnly();

    public Schedule(DateOnly validFrom, DateOnly validTo, IEnumerable<DayOfWeek> daysOfWeek)
    {
        ValidFrom = validFrom;
        ValidTo = validTo;
        _daysOfWeek = [..daysOfWeek];
        new ScheduleValidator().ValidateAndThrow(this);
    }
    
    private Schedule() {}
};