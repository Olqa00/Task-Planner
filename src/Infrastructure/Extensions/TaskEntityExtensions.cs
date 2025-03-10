namespace Planner.Infrastructure.Extensions;

using Planner.Application.Results;
using Planner.Domain.Entities;
using Planner.Infrastructure.Models;

internal static class TaskEntityExtensions
{
    public static IReadOnlyList<TaskDbEntity> ToDbEntities(this IReadOnlyList<TaskEntity> entities)
    {
        return entities.Select(entity => entity.ToDbEntity())
            .ToList();
    }

    public static TaskDbEntity ToDbEntity(this TaskEntity entity)
    {
        return new TaskDbEntity
        {
            Id = entity.Id.Value,
            Title = entity.Title,
            CreatedAt = entity.CreatedAt,
            CompletedAt = entity.CompletedAt,
        };
    }

    public static TaskResult ToResult(this TaskEntity entity)
    {
        return new TaskResult
        {
            CompletedAt = entity.CompletedAt,
            CreatedAt = entity.CreatedAt,
            Id = entity.Id.Value,
            IsComplete = entity.IsCompleted,
            Title = entity.Title,
        };
    }

    public static IReadOnlyList<TaskResult> ToResults(this IReadOnlyList<TaskEntity> entities)
    {
        return entities.Select(entity => entity.ToResult())
            .ToList();
    }
}
