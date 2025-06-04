namespace TrackEasy.Application.Api;

public sealed record LatestResponse(Dictionary<string, string>? Rates, string? Base, string? Date);