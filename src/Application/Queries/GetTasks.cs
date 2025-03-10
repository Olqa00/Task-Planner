namespace Planner.Application.Queries;

using Planner.Application.Results;

public sealed record class GetTasks : IRequest<IReadOnlyList<TaskResult>>
{
}
