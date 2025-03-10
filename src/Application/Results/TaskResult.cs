namespace Planner.Application.Results;

public sealed record class TaskResult
{
    public DateTime? CompletedAt { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required Guid Id { get; init; }
    public required bool IsComplete { get; init; }
    public required string Title { get; init; }
}
