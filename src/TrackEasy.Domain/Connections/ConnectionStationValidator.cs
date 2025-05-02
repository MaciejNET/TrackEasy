using FluentValidation;

namespace TrackEasy.Domain.Connections;

internal sealed class ConnectionStationValidator : AbstractValidator<ConnectionStation>
{
    public ConnectionStationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Station)
            .NotEmpty();

        RuleFor(x => x.ArrivalTime)
            .NotEmpty()
            .When(x => x.DepartureTime is null);
        
        RuleFor(x => x.DepartureTime)
            .NotEmpty()
            .When(x => x.ArrivalTime is null);
        
        RuleFor(x => x)
            .Must(x => x.ArrivalTime == null || x.DepartureTime == null || x.ArrivalTime <= x.DepartureTime)
            .WithMessage("Arrival time must be less than or equal to departure time.");
    }
}