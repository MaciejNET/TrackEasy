using TrackEasy.Domain.Discounts;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Discounts.UpdateDiscount;

internal sealed class UpdateDiscountCommandHandler(IDiscountRepository discountRepository)
    : ICommandHandler<UpdateDiscountCommand>
{
    public async Task Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await discountRepository.GetByIdAsync(request.Id, cancellationToken);

        if (discount is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Discount with id: {request.Id} does not exist.");
        }

        discount.Update(request.Name, request.Percentage);
        await discountRepository.SaveChangesAsync(cancellationToken);
    }
}