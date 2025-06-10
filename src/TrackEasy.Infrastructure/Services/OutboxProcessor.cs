using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Infrastructure.Services;

internal sealed class OutboxProcessor(IServiceProvider serviceProvider, ILogger<OutboxProcessor> logger, TimeProvider timeProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TrackEasyDbContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var messages = await dbContext.OutboxMessages
                .Where(x => x.ProcessedOn == null)
                .OrderBy(x => x.OccurredOn)
                .Take(20)
                .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                try
                {
                    var type = Type.GetType(message.Type);
                    if (type is null)
                    {
                        message.Error = $"Type {message.Type} not found";
                        continue;
                    }

                    var domainEvent = (IDomainEvent?)JsonSerializer.Deserialize(message.Content, type);
                    if (domainEvent is null)
                    {
                        message.Error = $"Could not deserialize {message.Type}";
                        continue;
                    }

                    await mediator.Publish(domainEvent, stoppingToken);
                    message.ProcessedOn = timeProvider.GetUtcNow().DateTime;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to process outbox message {MessageId}", message.Id);
                    message.Error = ex.Message;
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
