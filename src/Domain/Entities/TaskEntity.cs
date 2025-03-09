namespace Planner.Domain.Entities;

using Planner.Domain.Exceptions;

public sealed class TaskEntity
{
    public DateTime? CompletedAt { get; private set; }
    public DateTime CreatedAt { get; private init; }
    public TaskId Id { get; private init; }
    public bool IsCompleted => this.CompletedAt.HasValue;
    public string Title { get; private set; }

    public TaskEntity(TaskId id, string title, DateTime createdAt, DateTime? completedAt)
    {
        this.Id = id;
        this.CreatedAt = createdAt;
        this.CompletedAt = completedAt;
        this.SetTitle(title);
    }

    public void Complete(DateTime completedAt)
    {
        if (this.IsCompleted is true)
        {
            throw new TaskAlreadyCompletedException(this.Id);
        }

        if (this.CreatedAt > completedAt)
        {
            throw new TaskCompletedBeforeCreationException(this.Id);
        }

        this.CompletedAt = completedAt;
    }

    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new TaskEmptyTitleException(this.Id);
        }

        this.Title = title;
    }

    public void UnComplete()
    {
        if (this.IsCompleted is false)
        {
            throw new TaskNotCompletedException(this.Id);
        }

        this.CompletedAt = null;
    }
}
