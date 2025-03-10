namespace Planner.Application.Commands;

public sealed class UpdateTask : IRequest
{
    public DateTime? CompletedAt { get; init; }
    public required Guid Id { get; init; }
    public required string Title { get; init; }
}
