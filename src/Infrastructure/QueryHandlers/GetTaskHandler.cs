namespace Planner.Infrastructure.QueryHandlers;

using Planner.Application.Queries;
using Planner.Application.Results;
using Planner.Domain.Interfaces;
using Planner.Infrastructure.Extensions;

internal sealed class GetTaskHandler : IRequestHandler<GetTask, TaskResult>
{
    private readonly ILogger<GetTaskHandler> logger;
    private readonly ITaskRepository taskRepository;

    public GetTaskHandler(ILogger<GetTaskHandler> logger, ITaskRepository taskRepository)
    {
        this.logger = logger;
        this.taskRepository = taskRepository;
    }

    public async Task<TaskResult> Handle(GetTask request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            (nameof(TaskId), request.Id)
        );

        this.logger.LogInformation("Try to get task");

        var taskId = new TaskId(request.Id);

        var entity = await this.taskRepository.GetByIdAsync(taskId);

        var result = entity?.ToResult();

        return result;
    }
}
