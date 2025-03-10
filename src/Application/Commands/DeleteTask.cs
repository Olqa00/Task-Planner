namespace Planner.Application.Commands;

public sealed class DeleteTask : IRequest
{
    public required Guid Id { get; init; }
}
