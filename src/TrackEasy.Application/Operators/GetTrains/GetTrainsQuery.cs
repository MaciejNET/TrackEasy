using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.Operators.GetTrains;

public sealed record GetTrainsQuery(Guid OperatorId, string? Name, int PageNumber, int PageSize) : IQuery<PaginatedResult<TrainDto>>;