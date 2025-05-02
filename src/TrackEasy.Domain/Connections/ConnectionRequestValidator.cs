using FluentValidation;

namespace TrackEasy.Domain.Connections;

internal sealed class ConnectionRequestValidator : AbstractValidator<ConnectionRequest>
{
    public ConnectionRequestValidator()
    {
        RuleFor(x => x.RequestType).IsInEnum();

        When(x => x.RequestType is ConnectionRequestType.ADD or ConnectionRequestType.DELETE, () =>
        {
            RuleFor(x => x.Schedule).Null();
            RuleFor(x => x.Stations).Null();
        });

        When(x => x.RequestType is ConnectionRequestType.UPDATE, () =>
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Schedule).NotNull();
            RuleFor(x => x.Stations!).ValidStations();
        });
    }
}