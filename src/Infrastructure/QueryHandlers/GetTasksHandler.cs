namespace Planner.Infrastructure.QueryHandlers;

using Planner.Application.Queries;
using Planner.Application.Results;
using Planner.Domain.Interfaces;
using Planner.Infrastructure.Extensions;

internal sealed class GetTasksHandler : IRequestHandler<GetTasks, IReadOnlyList<TaskResult>>
{
    private readonly ILogger<GetTasksHandler> logger;
    private readonly ITaskRepository taskRepository;

    public GetTasksHandler(ILogger<GetTasksHandler> logger, ITaskRepository taskRepository)
    {
        this.logger = logger;
        this.taskRepository = taskRepository;
    }

    public async Task<IReadOnlyList<TaskResult>> Handle(GetTasks request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get tasks.");

        var entities = await this.taskRepository.GetTasksAsync();

        var result = entities.ToResults();

        return result;
    }
}
