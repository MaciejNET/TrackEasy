using MediatR;
using TrackEasy.Infrastructure.Database;

namespace TrackEasy.Infrastructure.Behaviors;

internal sealed class TransactionalBehavior<TRequest, TResponse>(TrackEasyDbContext dbContext) 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.GetType().Name.EndsWith("Query"))
        {
            return await next();
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var response = await next();
            
            await transaction.CommitAsync(cancellationToken);

            return response;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}