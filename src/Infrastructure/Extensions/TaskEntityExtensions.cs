namespace Planner.Infrastructure.Extensions;

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
}
