using System;
using System.Linq;
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
    private readonly TimeProvider _timeProvider = timeProvider;
    
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

        ValidateCard(request);

        var intentOps = new PaymentIntentCreateOptions
        {
            Amount = totalPrice.ToMinorUnits(),
            Currency = totalPrice.GetCurrencyCode(),
            ReturnUrl = "https://example.com/payment-complete",
            Confirm = true,
            PaymentMethod = "pm_card_visa",
            PaymentMethodTypes = ["card"]
        };

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

        tickets.ForEach(x => x.Pay(_timeProvider, intent.Id));
        await ticketRepository.SaveChangesAsync(cancellationToken);
    }

    private void ValidateCard(PayTicketByCardCommand request)
    {
        bool numberValid = request.CardNumber == "4242424242424242";
        bool cvcValid = request.CardCvc.All(char.IsDigit) &&
                        (request.CardCvc.Length == 3 || request.CardCvc.Length == 4);

        DateTimeOffset now = _timeProvider.GetUtcNow();
        bool expiryValid;
        try
        {
            var expiry = new DateTime(request.CardExpYear, request.CardExpMonth, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddMonths(1)
                .AddTicks(-1);
            expiryValid = expiry >= now;
        }
        catch (Exception)
        {
            expiryValid = false;
        }

        if (!numberValid || !cvcValid || !expiryValid)
        {
            throw new TrackEasyException(SharedCodes.InvalidInput, "Invalid card details.");
        }
    }
}