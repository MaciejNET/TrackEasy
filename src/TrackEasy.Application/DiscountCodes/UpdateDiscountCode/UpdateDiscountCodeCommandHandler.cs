using TrackEasy.Domain.DiscountCodes;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.DiscountCodes.UpdateDiscountCode;

internal sealed class UpdateDiscountCodeCommandHandler(IDiscountCodeRepository discountCodeRepository, TimeProvider timeProvider) 
    : ICommandHandler<UpdateDiscountCodeCommand>
{
    public async Task Handle(UpdateDiscountCodeCommand request, CancellationToken cancellationToken)
    {
        var discountCode = await discountCodeRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (discountCode is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Discount code with id: {request.Id} does not exists.");
        }
        
        discountCode.Update(request.Percentage, request.From, request.To, timeProvider);
        await discountCodeRepository.SaveChangesAsync(cancellationToken);
    }
}