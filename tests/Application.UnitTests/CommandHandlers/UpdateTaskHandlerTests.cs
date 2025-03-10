namespace Planner.Application.UnitTests.CommandHandlers;

using Planner.Application.CommandHandlers;
using Planner.Application.Commands;
using Planner.Application.Exceptions;
using Planner.Domain.Entities;
using Planner.Domain.Interfaces;
using Planner.Domain.Types;

[TestClass]
public sealed class UpdateTaskHandlerTests
{
    private const string NEW_TITLE = "Boring task title";
    private const string TITLE = "Super task title";
    private static readonly DateTime COMPLETED_AT = new(year: 2024, month: 04, day: 07, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT = new(year: 2024, month: 03, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid ID_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(ID_GUID);
    private readonly UpdateTaskHandler handler;

    private readonly NullLogger<UpdateTaskHandler> logger = new();
    private readonly ITaskRepository repository = Substitute.For<ITaskRepository>();

    public UpdateTaskHandlerTests()
    {
        this.handler = new UpdateTaskHandler(this.logger, this.repository);
    }

    [TestMethod]
    public void Handle_Should_ThrowTaskNotFoundException_WhenTaskDoesNotExist()
    {
        // Arrange
        this.repository.GetByIdAsync(Arg.Any<TaskId>())
            .Returns(Task.FromResult<TaskEntity?>(null));

        var command = new UpdateTask
        {
            CompletedAt = COMPLETED_AT,
            Id = ID_GUID,
            Title = NEW_TITLE,
        };

        // Act
        var action = async () => await this.handler.Handle(command, CancellationToken.None);

        // Assert
        action.Should()
            .ThrowAsync<TaskNotFoundException>()
            .WithMessage($"Task with id {ID_GUID} was not found")
            ;
    }

    [TestMethod]
    public async Task Handle_Should_UpdateTask_WhenTaskExists()
    {
        // Arrange
        TaskEntity? updatedTaskEntity = null;

        var taskEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, completedAt: null);

        this.repository.GetByIdAsync(Arg.Any<TaskId>())
            .Returns(taskEntity);

        await this.repository.UpdateAsync(Arg.Do<TaskEntity>(task => updatedTaskEntity = task));

        var command = new UpdateTask
        {
            CompletedAt = COMPLETED_AT,
            Id = ID_GUID,
            Title = NEW_TITLE,
        };

        // Act
        await this.handler.Handle(command, CancellationToken.None);

        // Assert
        var expectedEntity = new TaskEntity(TASK_ID, NEW_TITLE, CREATED_AT, COMPLETED_AT);

        updatedTaskEntity.Should()
            .BeEquivalentTo(expectedEntity)
            ;
    }
}
