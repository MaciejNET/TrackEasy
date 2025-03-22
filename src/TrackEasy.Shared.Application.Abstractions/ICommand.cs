using MediatR;

namespace TrackEasy.Shared.Application.Abstractions;

public interface ICommand : IRequest;

public interface ICommand<out TResponse> : IRequest<TResponse>;
