namespace Planner.Application.UnitTests.CommandHandlers;

using Planner.Application.CommandHandlers;
using Planner.Application.Commands;
using Planner.Application.Exceptions;
using Planner.Domain.Entities;
using Planner.Domain.Interfaces;
using Planner.Domain.Types;

[TestClass]
public sealed class DeleteTaskHandlerTests
{
    private const string TITLE = "Super task title";
    private static readonly DateTime COMPLETED_AT = new(year: 2024, month: 04, day: 07, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT = new(year: 2024, month: 03, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid ID_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(ID_GUID);
    private readonly DeleteTaskHandler handler;

    private readonly NullLogger<DeleteTaskHandler> logger = new();
    private readonly ITaskRepository repository = Substitute.For<ITaskRepository>();

    public DeleteTaskHandlerTests()
    {
        this.handler = new DeleteTaskHandler(this.logger, this.repository);
    }

    [TestMethod]
    public async Task Handle_Should_DeleteTask_WhenTaskExists()
    {
        // Arrange
        TaskId? deletedTaskId = null;

        var taskEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, completedAt: null);

        this.repository.GetByIdAsync(Arg.Any<TaskId>())
            .Returns(taskEntity);

        await this.repository.DeleteAsync(Arg.Do<TaskId>(id => deletedTaskId = id));

        var command = new DeleteTask
        {
            Id = ID_GUID,
        };

        // Act
        await this.handler.Handle(command, CancellationToken.None);

        // Assert
        deletedTaskId.Should()
            .BeEquivalentTo(TASK_ID)
            ;
    }

    [TestMethod]
    public void Handle_Should_ThrowTaskNotFoundException_WhenTaskDoesNotExist()
    {
        // Arrange
        this.repository.GetByIdAsync(Arg.Any<TaskId>())
            .Returns(Task.FromResult<TaskEntity?>(null));

        var command = new DeleteTask
        {
            Id = ID_GUID,
        };

        // Act
        var action = async () => await this.handler.Handle(command, CancellationToken.None);

        // Assert
        action.Should()
            .ThrowAsync<TaskNotFoundException>()
            .WithMessage($"Task with id {ID_GUID} was not found")
            ;
    }
}
