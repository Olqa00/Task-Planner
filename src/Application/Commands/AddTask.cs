namespace Planner.Application.Commands;

public sealed record class AddTask : IRequest
{
    public required DateTime CreatedAt { get; init; }
    public required Guid Id { get; init; }
    public required string Title { get; init; }
}
