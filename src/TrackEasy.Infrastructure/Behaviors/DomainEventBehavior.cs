using MediatR;

namespace TrackEasy.Infrastructure.Behaviors;

internal sealed class DomainEventBehavior<TRequest, TResponse>(DomainEventDispatcher domainEventDispatcher)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.GetType().Name.EndsWith("Query"))
        {
            return await next();
        }
        
        var response = await next();

        await domainEventDispatcher.DispatchEventsAsync(cancellationToken);

        return response;
    }
}
