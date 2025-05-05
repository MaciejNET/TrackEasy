using FluentValidation;
using Shouldly;
using TrackEasy.Domain.Connections;

namespace TrackEasy.UnitTests.Connection;

public class ScheduleTests
{
    [Fact]
    public void CreateSchedule_WithValidData_ShouldSucceed()
    {
        var validFrom = new DateOnly(2023, 1, 1);
        var validTo = new DateOnly(2023, 12, 31);
        var daysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday };

        var schedule = new Schedule(validFrom, validTo, daysOfWeek);

        schedule.ValidFrom.ShouldBe(validFrom);
        schedule.ValidTo.ShouldBe(validTo);
        schedule.DaysOfWeek.ShouldBe(daysOfWeek);
    }

    [Fact]
    public void CreateSchedule_WithInvalidDateRange_ShouldThrowValidationException()
    {
        var validFrom = new DateOnly(2023, 12, 31);
        var validTo = new DateOnly(2023, 1, 1);
        var daysOfWeek = new List<DayOfWeek> { DayOfWeek.Tuesday };

        Should.Throw<ValidationException>(() => new Schedule(validFrom, validTo, daysOfWeek));
    }

    [Fact]
    public void CreateSchedule_WithEmptyDaysOfWeek_ShouldThrowValidationException()
    {
        var validFrom = new DateOnly(2023, 1, 1);
        var validTo = new DateOnly(2023, 12, 31);
        var daysOfWeek = Enumerable.Empty<DayOfWeek>();

        Should.Throw<ValidationException>(() => new Schedule(validFrom, validTo, daysOfWeek));
    }
}