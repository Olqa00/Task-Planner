namespace Planner.Domain.Exceptions;

internal sealed class TaskEmptyTitleException : DomainException
{
    public TaskEmptyTitleException(TaskId id)
        : base($"The Task {id.Value} has empty title.")
    {
        this.Id = id.Value;
    }
}
