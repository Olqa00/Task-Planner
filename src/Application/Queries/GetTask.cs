namespace Planner.Application.Queries;

using Planner.Application.Results;

public sealed record class GetTask : IRequest<TaskResult>
{
    public required Guid Id { get; init; }
}
