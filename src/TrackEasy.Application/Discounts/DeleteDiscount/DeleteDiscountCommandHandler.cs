using TrackEasy.Domain.Discounts;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Discounts.DeleteDiscount;

internal sealed class DeleteDiscountCommandHandler(IDiscountRepository discountRepository) : ICommandHandler<DeleteDiscountCommand>
{
    public async Task Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await discountRepository.GetByIdAsync(request.Id, cancellationToken);

        if (discount is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Discount with id: {request.Id} does not exist.");
        }

        discountRepository.Delete(discount);
        await discountRepository.SaveChangesAsync(cancellationToken);
    }
}