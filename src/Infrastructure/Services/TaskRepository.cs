﻿namespace Planner.Infrastructure.Services;

using Planner.Domain.Entities;
using Planner.Domain.Interfaces;
using Planner.Infrastructure.DAL;
using Planner.Infrastructure.Extensions;
using Planner.Infrastructure.Models;

internal sealed class TaskRepository : ITaskRepository
{
    private const string GET_ALL_QUERY =
        """
        SELECT
             Id
            ,Title
            ,CreatedAt
            ,CompletedAt
        FROM Task
        """;

    private const string GET_BY_ID_QUERY =
        """
        SELECT
             Id
            ,Title
            ,CreatedAt
            ,CompletedAt
        FROM Task
        WHERE Id = @Id
        """;

    private const string INSERT_COMMAND =
        """
        INSERT INTO Task (
             Id
            ,Title
            ,CreatedAt
            ,CompletedAt
        )
        VALUES (
             @Id
            ,@Title
            ,@CreatedAt
            ,@CompletedAt
        )
        """;

    private const string UPDATE_COMMAND =
        """
        UPDATE Task
        SET
             Title = @Title
            ,CompletedAt = @CompletedAt
        WHERE Id = @Id
        """;

    private readonly SqlServerOptions options;

    public TaskRepository(SqlServerOptions options)
    {
        this.options = options;
    }

    public async Task AddTaskAsync(TaskEntity task)
    {
        var dbModel = task.ToDbEntity();

        using IDbConnection db = new SqlConnection(this.options.ConnectionString);

        await db.ExecuteAsync(INSERT_COMMAND, dbModel);
    }

    public async Task<TaskEntity?> GetByIdAsync(TaskId id)
    {
        var parameters = new
        {
            Id = id.Value,
        };

        using IDbConnection db = new SqlConnection(this.options.ConnectionString);

        var dbEntity = await db.QueryFirstOrDefaultAsync<TaskDbEntity>(GET_BY_ID_QUERY, parameters);

        var entity = dbEntity?.ToEntity();

        return entity;
    }

    public async Task<IReadOnlyList<TaskEntity>> GetTasksAsync()
    {
        using IDbConnection db = new SqlConnection(this.options.ConnectionString);

        var dbEntities = await db.QueryAsync<TaskDbEntity>(GET_ALL_QUERY);

        var entities = dbEntities.ToEntities();

        return entities;
    }

    public async Task UpdateAsync(TaskEntity task)
    {
        var parameters = new
        {
            Id = task.Id.Value,
            task.Title,
            task.CompletedAt,
        };

        using IDbConnection db = new SqlConnection(this.options.ConnectionString);

        await db.ExecuteAsync(UPDATE_COMMAND, parameters);
    }
}
