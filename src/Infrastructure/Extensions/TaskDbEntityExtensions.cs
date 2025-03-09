namespace Planner.Infrastructure.Extensions;

using Planner.Domain.Entities;
using Planner.Infrastructure.Models;

internal static class TaskDbEntityExtensions
{
    public static IReadOnlyList<TaskEntity> ToEntities(this IEnumerable<TaskDbEntity> dbEntities)
    {
        return dbEntities.Select(dbEntity => dbEntity.ToEntity())
            .ToList();
    }

    public static TaskEntity ToEntity(this TaskDbEntity dbEntity)
    {
        var taskId = new TaskId(dbEntity.Id);

        var result = new TaskEntity(taskId, dbEntity.Title, dbEntity.CreatedAt, dbEntity.CompletedAt);

        return result;
    }
}
