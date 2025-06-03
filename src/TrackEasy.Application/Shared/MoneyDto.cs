using TrackEasy.Domain.Shared;

namespace TrackEasy.Application.Shared;

public sealed record MoneyDto(decimal Amount, Currency Currency);