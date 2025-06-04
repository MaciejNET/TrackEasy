using Refit;

namespace TrackEasy.Application.Api;

[Headers("Accept: application/json")]
public interface ICurrencyFreaksApi
{
    [Get("/v2.0/latest")]
    Task<LatestResponse> GetLatestRatesAsync(
        [AliasAs("apikey")] string apiKey,
        [AliasAs("symbols")] string symbols,
        CancellationToken ct = default);

}