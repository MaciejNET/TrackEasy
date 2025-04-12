using TrackEasy.Domain.Discounts;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Discounts.CreateDiscount;

internal sealed class CreateDiscountCommandHandler(IDiscountRepository discountRepository)
    : ICommandHandler<CreateDiscountCommand>
{
    public async Task Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var exists = await discountRepository.ExistsAsync(request.Name, cancellationToken);

        if (exists)
        {
            throw new TrackEasyException(Codes.DiscountAlreadyExists, "Discount with the same name already exists.");
        }

        var discount = Discount.Create(request.Name, request.Percentage);
        discountRepository.Add(discount);
        await discountRepository.SaveChangesAsync(cancellationToken);
    }
}