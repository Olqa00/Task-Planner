namespace Planner.Domain.Types;

public sealed class TaskId
{
    public Guid Value { get; init; }
    public TaskId(Guid value) => this.Value = value;
}
