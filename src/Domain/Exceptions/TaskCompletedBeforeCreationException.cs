namespace Planner.Domain.Exceptions;

internal sealed class TaskCompletedBeforeCreationException : DomainException
{
    public TaskCompletedBeforeCreationException(TaskId id)
        : base($"The task {id.Value} was completed before it was created.")
    {
        this.Id = id.Value;
    }
}
