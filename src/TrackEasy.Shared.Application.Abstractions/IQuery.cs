using MediatR;

namespace TrackEasy.Shared.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>;