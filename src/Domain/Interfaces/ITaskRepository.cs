﻿namespace Planner.Domain.Interfaces;

using Planner.Domain.Entities;

public interface ITaskRepository
{
    Task AddTaskAsync(TaskEntity task);
    Task DeleteAsync(TaskId id);
    Task<TaskEntity?> GetByIdAsync(TaskId id);
    Task<IReadOnlyList<TaskEntity>> GetTasksAsync();
    Task UpdateAsync(TaskEntity task);
}
