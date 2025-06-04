using TrackEasy.Domain.Shared;

namespace TrackEasy.Application.Services;

public interface ICurrencyService
{
    Task<Money> ConvertAsync(Money money, Currency toCurrency, CancellationToken cancellationToken);
}