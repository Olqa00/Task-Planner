namespace Planner.Infrastructure.UnitTests.QueryHandlers;

using Planner.Application.Queries;
using Planner.Application.Results;
using Planner.Domain.Entities;
using Planner.Domain.Interfaces;
using Planner.Infrastructure.QueryHandlers;

[TestClass]
public sealed class GetTaskHandlerTests
{
    private const string TITLE = "Super task title";
    private static readonly DateTime CREATED_AT = new(year: 2024, month: 03, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid ID_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(ID_GUID);

    private static readonly TaskResult TASK_RESULT = new()
    {
        CreatedAt = CREATED_AT,
        Id = ID_GUID,
        IsComplete = false,
        Title = TITLE,
    };

    private readonly GetTaskHandler handler;
    private readonly NullLogger<GetTaskHandler> logger = new();
    private readonly ITaskRepository repository = Substitute.For<ITaskRepository>();

    private readonly TaskEntity taskEntity;

    public GetTaskHandlerTests()
    {
        this.taskEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, completedAt: null);

        this.handler = new GetTaskHandler(this.logger, this.repository);
    }

    [TestMethod]
    public async Task Handle_Should_ReturnNull()
    {
        // Arrange
        this.repository.GetByIdAsync(TASK_ID)
            .Returns((TaskEntity)null);

        var query = new GetTask
        {
            Id = ID_GUID,
        };

        // Act
        var result = await this.handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should()
            .BeNull()
            ;
    }

    [TestMethod]
    public async Task Handle_Should_ReturnTask()
    {
        // Arrange
        this.repository.GetByIdAsync(Arg.Any<TaskId>())
            .Returns(this.taskEntity);

        var query = new GetTask
        {
            Id = ID_GUID,
        };

        // Act
        var result = await this.handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should()
            .BeEquivalentTo(TASK_RESULT)
            ;
    }
}
