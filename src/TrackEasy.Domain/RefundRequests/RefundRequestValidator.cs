using FluentValidation;

namespace TrackEasy.Domain.RefundRequests;

internal sealed class RefundRequestValidator : AbstractValidator<RefundRequest>
{
    public RefundRequestValidator()
    {
        RuleFor(x => x.Ticket)
            .NotNull();

        RuleFor(x => x.Reason)
            .MinimumLength(3)
            .MaximumLength(255);
    }
}