namespace Planner.Application.Exceptions;

public sealed class TaskAlreadyExistsException : ApplicationException
{
    public TaskAlreadyExistsException(TaskId id)
        : base($"Task with id {id.Value} already exists")
    {
        this.Id = id.Value;
    }
}
