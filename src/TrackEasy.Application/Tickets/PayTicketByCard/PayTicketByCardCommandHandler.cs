using Stripe;
using TrackEasy.Application.Services;
using TrackEasy.Domain.Shared;
using TrackEasy.Domain.Tickets;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Tickets.PayTicketByCard;

internal sealed class PayTicketByCardCommandHandler(
    ITicketRepository ticketRepository,
    StripeClient stripeClient,
    ICurrencyService currencyService,
    TimeProvider timeProvider) : ICommandHandler<PayTicketByCardCommand>
{
    private readonly PaymentIntentService _paymentIntentService = new(stripeClient);
    
    public async Task Handle(PayTicketByCardCommand request, CancellationToken cancellationToken)
    {
        List<Ticket> tickets = [];
        foreach (var ticketId in request.TicketIds)
        {
            var ticket = await ticketRepository.FindByIdAsync(ticketId, cancellationToken);
            if (ticket is null)
            {
                throw new TrackEasyException(SharedCodes.EntityNotFound, $"Ticket with ID {ticketId} was not found.");
            }
            
            if (ticket.TicketStatus != TicketStatus.PENDING)
            {
                throw new TrackEasyException(SharedCodes.InvalidInput, "Ticket is not in a payable state.");
            }
            
            tickets.Add(ticket);
        }

        Money totalPrice = new(0, request.Currency);
        foreach (var ticket in tickets)
        {
            var price = ticket.Price.Currency == request.Currency
                ? ticket.Price
                : await currencyService.ConvertAsync(ticket.Price, request.Currency, cancellationToken);
            
            totalPrice += price;
        }

        var intentOps = new PaymentIntentCreateOptions
        {
            Amount = totalPrice.ToMinorUnits(),
            Currency = totalPrice.GetCurrencyCode(),
            Confirm = true,
            PaymentMethodData = new PaymentIntentPaymentMethodDataOptions
            {
                Type = "card"
            }
        };

        intentOps.AddExtraParam("payment_method_data[card][number]", request.CardNumber);
        intentOps.AddExtraParam("payment_method_data[card][exp_month]", request.CardExpMonth.ToString());
        intentOps.AddExtraParam("payment_method_data[card][exp_year]", request.CardExpYear.ToString());
        intentOps.AddExtraParam("payment_method_data[card][cvc]", request.CardCvc);

        PaymentIntent intent;
        try
        {
            intent = await _paymentIntentService.CreateAsync(intentOps, cancellationToken: cancellationToken);
        }
        catch (StripeException)
        {
            throw new TrackEasyException(Codes.PaymentFailed, "Payment failed. Please check your card details and try again.");
        }
        
        if (intent.Status != "succeeded")
        {
            throw new TrackEasyException(Codes.PaymentFailed, "Payment was not successful.");
        }

        tickets.ForEach(x => x.Pay(timeProvider, intent.Id));
        await ticketRepository.SaveChangesAsync(cancellationToken);
    }
}