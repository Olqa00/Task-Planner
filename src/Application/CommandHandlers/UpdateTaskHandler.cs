namespace Planner.Application.CommandHandlers;

using Planner.Application.Commands;
using Planner.Application.Exceptions;
using Planner.Domain.Entities;
using Planner.Domain.Interfaces;

internal sealed class UpdateTaskHandler : IRequestHandler<UpdateTask>
{
    private readonly ILogger<UpdateTaskHandler> logger;
    private readonly ITaskRepository repository;

    public UpdateTaskHandler(ILogger<UpdateTaskHandler> logger, ITaskRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(UpdateTask request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            (nameof(TaskId), request.Id)
        );

        this.logger.LogInformation("Try to update task");

        var taskId = new TaskId(request.Id);

        var task = await this.repository.GetByIdAsync(taskId);

        if (task is null)
        {
            throw new TaskNotFoundException(taskId);
        }

        var taskEntity = new TaskEntity(taskId, request.Title, task.CreatedAt, request.CompletedAt);

        await this.repository.UpdateAsync(taskEntity);
    }
}
