namespace Planner.Application.Exceptions;

public sealed class TaskNotFoundException : ApplicationException
{
    public TaskNotFoundException(TaskId id)
        : base($"Task with id {id.Value} not found")
    {
        this.Id = id.Value;
    }
}
