using TrackEasy.Domain.DiscountCodes;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.DiscountCodes.CreateDiscountCode;

internal sealed class CreateDiscountCodeCommandHandler(IDiscountCodeRepository discountCodeRepository, TimeProvider timeProvider)
    : ICommandHandler<CreateDiscountCodeCommand>
{
    public async Task Handle(CreateDiscountCodeCommand request, CancellationToken cancellationToken)
    {
        var exists = await discountCodeRepository.ExistsAsync(request.Code, cancellationToken);

        if (exists)
        {
            throw new TrackEasyException(Codes.DiscountCodeAlreadyExists, "Discount code already exists.");
        }
        
        var discountCode = DiscountCode.Create(request.Code, request.Percentage, request.From, request.To, timeProvider);
        discountCodeRepository.Add(discountCode);
        await discountCodeRepository.SaveChangesAsync(cancellationToken);
    }
}