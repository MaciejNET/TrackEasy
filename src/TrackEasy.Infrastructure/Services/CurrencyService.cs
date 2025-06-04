using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using TrackEasy.Application.Api;
using TrackEasy.Application.Services;
using TrackEasy.Domain.Shared;

namespace TrackEasy.Infrastructure.Services;

internal sealed class CurrencyService(ICurrencyFreaksApi api, IMemoryCache cache, IConfiguration cfg) : ICurrencyService
{
    private readonly string _apiKey = cfg["currency-freaks-api-key"] ?? throw new InvalidOperationException("Currency Freaks API key is not configured.");
    public async Task<Money> ConvertAsync(Money money, Currency toCurrency, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if (money.Currency == toCurrency)
            return money;
        
        var cacheKey = $"{money.Currency}-{toCurrency}";
        if (!cache.TryGetValue(cacheKey, out decimal fx))
        {
            fx = await RetrieveRateAsync(money.Currency, toCurrency, cancellationToken);
            cache.Set(cacheKey, fx, TimeSpan.FromMinutes(30));
        }
        
        var converted = decimal.Round(money.Amount * fx, 2, MidpointRounding.ToEven);
        return new Money(converted, toCurrency);
    }
    
    private async Task<decimal> RetrieveRateAsync(Currency from, Currency to, CancellationToken ct)
    {
        var symbols = $"{from},{to}";
        var resp    = await api.GetLatestRatesAsync(_apiKey, symbols, ct);

        if (resp.Rates is null)
            throw new InvalidOperationException("CurrencyFreaks response did not include 'rates'.");

        if (!resp.Rates.TryGetValue(from.ToString(), out var strFromRate) ||
            !resp.Rates.TryGetValue(to.ToString(),   out var strToRate))
        {
            throw new InvalidOperationException($"CurrencyFreaks did not return both {from} and {to}.");
        }

        if (!decimal.TryParse((string?)strFromRate, NumberStyles.Any, CultureInfo.InvariantCulture, out var usdToFrom) ||
            !decimal.TryParse((string?)strToRate,   NumberStyles.Any, CultureInfo.InvariantCulture, out var usdToTo))
        {
            throw new InvalidOperationException("Failed to parse rate values from CurrencyFreaks.");
        }

        if (usdToFrom == 0)
            throw new InvalidOperationException($"Rate USD→{from} is zero – cannot compute reciprocal.");

        return usdToTo / usdToFrom;
    }
}