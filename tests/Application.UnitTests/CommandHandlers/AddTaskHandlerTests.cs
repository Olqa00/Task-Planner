namespace Planner.Application.UnitTests.CommandHandlers;

using Planner.Application.CommandHandlers;
using Planner.Application.Commands;
using Planner.Application.Exceptions;
using Planner.Domain.Entities;
using Planner.Domain.Interfaces;
using Planner.Domain.Types;

[TestClass]
public sealed class AddTaskHandlerTests
{
    private const string TITLE = "Super task title";
    private static readonly DateTime CREATED_AT = new(year: 2024, month: 03, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid ID_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(ID_GUID);
    private readonly AddTaskHandler handler;

    private readonly NullLogger<AddTaskHandler> logger = new();
    private readonly ITaskRepository repository = Substitute.For<ITaskRepository>();

    public AddTaskHandlerTests()
    {
        this.handler = new AddTaskHandler(this.logger, this.repository);
    }

    [TestMethod]
    public async Task Handle_Should_AddTask_WhenTaskDoesNotExist()
    {
        // Arrange
        TaskEntity? taskEntity = null;

        this.repository.GetByIdAsync(TASK_ID)
            .Returns(Task.FromResult<TaskEntity?>(null));

        await this.repository.AddTaskAsync(Arg.Do<TaskEntity>(task => taskEntity = task));

        var command = new AddTask
        {
            CreatedAt = CREATED_AT,
            Id = ID_GUID,
            Title = TITLE,
        };

        // Act
        await this.handler.Handle(command, CancellationToken.None);

        // Assert
        var expectedEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, completedAt: null);

        taskEntity.Should()
            .BeEquivalentTo(expectedEntity)
            ;
    }

    [TestMethod]
    public void Handle_Should_ThrowTaskAlreadyExistsException_WhenTaskExists()
    {
        // Arrange
        var taskEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, completedAt: null);

        this.repository.GetByIdAsync(TASK_ID)
            .Returns(taskEntity);

        var command = new AddTask
        {
            CreatedAt = CREATED_AT,
            Id = ID_GUID,
            Title = TITLE,
        };

        // Act
        var action = async () => await this.handler.Handle(command, CancellationToken.None);

        // Assert
        action.Should()
            .ThrowAsync<TaskAlreadyExistsException>()
            .WithMessage($"Task with id {ID_GUID} already exists")
            ;
    }
}
