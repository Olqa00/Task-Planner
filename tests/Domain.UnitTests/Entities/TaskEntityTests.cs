namespace Planner.Domain.UnitTests.Entities;

using Planner.Domain.Entities;
using Planner.Domain.Exceptions;

[TestClass]
public sealed class TaskEntityTests
{
    private const string TASK_TITLE = "Task Title";
    private const string TASK_TITLE_2 = "New Task Title";
    private static readonly DateTime COMPETED_AT = new(year: 2025, month: 03, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 03, day: 04, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid TASK_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(TASK_GUID);
    private static readonly DateTime WRONG_DATE = new(year: 2024, month: 03, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);

    [TestMethod]
    public void Complete_ThrowsException_WhenTaskCompletedBeforeCreation()
    {
        // Arrange
        var task = new TaskEntity(TASK_ID, TASK_TITLE, CREATED_AT, completedAt: null);

        // Act
        var act = () => task.Complete(WRONG_DATE);

        // Assert
        act.Should()
            .Throw<TaskCompletedBeforeCreationException>()
            ;
    }

    [TestMethod]
    public void Complete_ThrowsException_WhenTaskIsAlreadyCompleted()
    {
        // Arrange
        var task = new TaskEntity(TASK_ID, TASK_TITLE, CREATED_AT, COMPETED_AT);

        // Act
        var act = () => task.Complete(COMPETED_AT);

        // Assert
        act.Should()
            .Throw<TaskAlreadyCompletedException>()
            ;
    }

    [TestMethod]
    public void SetTitle_Should_ChangeTitle()
    {
        // Arrange
        var task = new TaskEntity(TASK_ID, TASK_TITLE, CREATED_AT, completedAt: null);

        // Act
        var act = () => task.SetTitle(TASK_TITLE_2);

        // Assert
        act.Should()
            .NotThrow()
            ;

        task.Title.Should()
            .Be(TASK_TITLE_2)
            ;
    }

    [DataTestMethod, DataRow(""), DataRow("       ")]
    public void SetTitle_ThrowsException_WhenTitleIsEmpty(string title)
    {
        // Arrange
        var task = new TaskEntity(TASK_ID, TASK_TITLE, CREATED_AT, completedAt: null);

        // Act
        var act = () => task.SetTitle(title);

        // Assert
        act.Should()
            .Throw<TaskEmptyTitleException>()
            ;
    }

    [TestMethod]
    public void UnComplete_Should_CompleteTask_WhenTaskCanBeCompleted()
    {
        // Arrange
        var task = new TaskEntity(TASK_ID, TASK_TITLE, CREATED_AT, completedAt: null);
        task.Complete(COMPETED_AT);

        // Act
        task.UnComplete();

        // Assert
        task.CompletedAt.Should()
            .BeNull()
            ;
    }

    [TestMethod]
    public void UnComplete_ThrowsException_WhenTaskIsNotCompleted()
    {
        // Arrange
        var task = new TaskEntity(TASK_ID, TASK_TITLE, CREATED_AT, completedAt: null);

        // Act
        var act = () => task.UnComplete();

        // Assert
        act.Should()
            .Throw<TaskNotCompletedException>()
            ;
    }
}
