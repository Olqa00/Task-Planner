namespace Planner.Infrastructure.Models;

public sealed record class TaskDbEntity
{
    public DateTime? CompletedAt { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required Guid Id { get; init; }
    public required string Title { get; init; }
}
