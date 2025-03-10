namespace Planner.Application.CommandHandlers;

using Planner.Application.Commands;
using Planner.Application.Exceptions;
using Planner.Domain.Interfaces;

internal sealed class DeleteTaskHandler : IRequestHandler<DeleteTask>
{
    private readonly ILogger<DeleteTaskHandler> logger;
    private readonly ITaskRepository repository;

    public DeleteTaskHandler(ILogger<DeleteTaskHandler> logger, ITaskRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(DeleteTask request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            (nameof(TaskId), request.Id)
        );

        this.logger.LogInformation("Try to delete task");

        var taskId = new TaskId(request.Id);

        var task = await this.repository.GetByIdAsync(taskId);

        if (task is null)
        {
            throw new TaskNotFoundException(taskId);
        }

        await this.repository.DeleteAsync(taskId);
    }
}
