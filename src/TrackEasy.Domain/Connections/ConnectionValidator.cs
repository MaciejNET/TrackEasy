using FluentValidation;

namespace TrackEasy.Domain.Connections;

internal sealed class ConnectionValidator : AbstractValidator<Connection>
{
    public ConnectionValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(15);

        RuleFor(x => x)
            .Must(x => x.Name.StartsWith(x.Operator.Code, StringComparison.InvariantCultureIgnoreCase));

        RuleFor(x => x.Operator)
            .NotEmpty();
        
        RuleFor(x => x)
            .Must(x => x.Operator.Trains.Contains(x.Train))
            .WithMessage("Train must belong to the operator");
        
        RuleFor(x => x.PricePerKilometer)
            .NotEmpty();
        
        RuleFor(x => x.Schedule)
            .NotEmpty();

        RuleFor(x => x.Stations)
            .ValidStations();
    }
}