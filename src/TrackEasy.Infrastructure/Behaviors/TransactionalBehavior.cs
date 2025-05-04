using MediatR;
using TrackEasy.Infrastructure.Database;

namespace TrackEasy.Infrastructure.Behaviors;

internal sealed class TransactionalBehavior<TRequest, TResponse>(TrackEasyDbContext dbContext) 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.GetType().Name.EndsWith("Query"))
        {
            return await next();
        }

        var executionStrategy = dbContext.Database.CreateExecutionStrategy();
        
        return await executionStrategy.ExecuteAsync(
            state: (dbContext, next, request),
            operation: async (context, state, ct) =>
            {
                var (currentDbContext, handler, _) = state;
                await using var transaction = await currentDbContext.Database.BeginTransactionAsync(ct);
                try
                {
                    var response = await handler();
                    await transaction.CommitAsync(ct);
                    return response;
                }
                catch
                {
                    await transaction.RollbackAsync(ct);
                    throw;
                }
            },
            verifySucceeded: null,
            cancellationToken: cancellationToken
        );
    }
}