namespace Planner.Infrastructure.UnitTests.QueryHandlers;

using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Planner.Application.Queries;
using Planner.Domain.Entities;
using Planner.Domain.Interfaces;
using Planner.Infrastructure.QueryHandlers;

[TestClass]
public sealed class GetTasksHandlerTests
{
    private const string TITLE_1 = "Super task title 1";
    private const string TITLE_2 = "Super task title 1";
    private static readonly DateTime CREATED_AT = new(year: 2024, month: 03, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid ID_GUID_1 = Guid.NewGuid();
    private static readonly Guid ID_GUID_2 = Guid.NewGuid();
    private static readonly TaskId TASK_ID_1 = new(ID_GUID_1);
    private static readonly TaskId TASK_ID_2 = new(ID_GUID_2);
    private readonly GetTasksHandler handler;
    private readonly NullLogger<GetTasksHandler> logger = new();
    private readonly ITaskRepository repository = Substitute.For<ITaskRepository>();

    private readonly TaskEntity taskEntity1;
    private readonly TaskEntity taskEntity2;

    public GetTasksHandlerTests()
    {
        this.taskEntity1 = new TaskEntity(TASK_ID_1, TITLE_1, CREATED_AT, completedAt: null);
        this.taskEntity2 = new TaskEntity(TASK_ID_2, TITLE_2, CREATED_AT, completedAt: null);

        this.handler = new GetTasksHandler(this.logger, this.repository);
    }

    [TestMethod]
    public async Task Handle_Should_ReturnEmptyList()
    {
        // Arrange
        this.repository.GetTasksAsync()
            .Returns(new List<TaskEntity>());

        var query = new GetTasks();

        // Act
        var result = await this.handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should()
            .BeEmpty()
            ;
    }

    [TestMethod]
    public async Task Handle_Should_ReturnTasks()
    {
        // Arrange
        var entities = new List<TaskEntity>
        {
            this.taskEntity1,
            this.taskEntity2,
        };

        this.repository.GetTasksAsync()
            .Returns(entities);

        var query = new GetTasks();

        // Act
        var result = await this.handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should()
            .HaveCount(2)
            ;
    }
}
