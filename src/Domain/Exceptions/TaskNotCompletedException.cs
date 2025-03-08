namespace Planner.Domain.Exceptions;

internal sealed class TaskNotCompletedException : DomainException
{
    public TaskNotCompletedException(TaskId id)
        : base($"The task {id.Value} has not been completed.")
    {
        this.Id = id.Value;
    }
}
