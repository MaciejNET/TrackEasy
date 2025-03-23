using TrackEasy.Domain.DiscountCodes;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.DiscountCodes.DeleteDiscountCode;

internal sealed class DeleteDiscountCodeCommandHandler(IDiscountCodeRepository discountCodeRepository) : ICommandHandler<DeleteDiscountCodeCommand>
{
    public async Task Handle(DeleteDiscountCodeCommand request, CancellationToken cancellationToken)
    {
        var discountCode = await discountCodeRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (discountCode is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Discount code with id: {request.Id} does not exists.");
        }
        
        discountCodeRepository.Delete(discountCode);
        await discountCodeRepository.SaveChangesAsync(cancellationToken);
    }
}