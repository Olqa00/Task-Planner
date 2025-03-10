namespace Planner.Application.CommandHandlers;

using Planner.Application.Commands;
using Planner.Application.Exceptions;
using Planner.Domain.Entities;
using Planner.Domain.Interfaces;

internal sealed class AddTaskHandler : IRequestHandler<AddTask>
{
    private readonly ILogger<AddTaskHandler> logger;
    private readonly ITaskRepository taskRepository;

    public AddTaskHandler(ILogger<AddTaskHandler> logger, ITaskRepository taskRepository)
    {
        this.logger = logger;
        this.taskRepository = taskRepository;
    }

    public async Task Handle(AddTask request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            (nameof(TaskId), request.Id)
        );

        this.logger.LogInformation("Try to add task");

        var taskId = new TaskId(request.Id);

        var existingTask = await this.taskRepository.GetByIdAsync(taskId);

        if (existingTask is not null)
        {
            throw new TaskAlreadyExistsException(taskId);
        }

        var taskEntity = new TaskEntity(taskId, request.Title, request.CreatedAt, completedAt: null);

        await this.taskRepository.AddTaskAsync(taskEntity);
    }
}
