using TrackEasy.Application.Operators.Shared;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.Operators.GetOperators;

public sealed record GetOperatorsQuery(int PageNumber, int PageSize) : IQuery<PaginatedResult<OperatorDto>>;