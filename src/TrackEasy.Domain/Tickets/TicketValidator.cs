using FluentValidation;

namespace TrackEasy.Domain.Tickets;

internal sealed class TicketValidator : AbstractValidator<Ticket>
{
    public TicketValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Price)
            .NotNull();

        RuleFor(x => x.ConnectionId)
            .NotEmpty();

        RuleFor(x => x.ConnectionName)
            .NotEmpty();

        When(x => x.SeatNumbers is not null, () =>
        {
            RuleFor(x => x.SeatNumbers)
                .NotEmpty();
        });

        RuleFor(x => x.OperatorId)
            .NotEmpty();

        RuleFor(x => x.OperatorCode)
            .NotEmpty();

        RuleFor(x => x.OperatorName)
            .NotEmpty();

        RuleFor(x => x.Stations)
            .NotEmpty();

        RuleFor(x => x.Passengers)
            .NotEmpty();

        When(x => x.SeatNumbers is not null, () =>
        {
            RuleFor(x => x)
                .Must(ticket => ticket.SeatNumbers!.Count == ticket.Passengers.Count);
        });

        RuleFor(x => x.EmailAddress)
            .EmailAddress();
    }
}