using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Infrastructure.Database;

public sealed class OutboxMessage
{
    public required Guid Id { get; set; }
    public required DateTime OccurredOn { get; set; }
    public required string Type { get; set; }
    public required string Content { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public string? Error { get; set; }
}
