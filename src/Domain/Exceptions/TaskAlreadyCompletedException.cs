namespace Planner.Domain.Exceptions;

internal sealed class TaskAlreadyCompletedException : DomainException
{
    public TaskAlreadyCompletedException(TaskId id)
        : base($"The task {id.Value} has already been completed.")
    {
        this.Id = id.Value;
    }
}
