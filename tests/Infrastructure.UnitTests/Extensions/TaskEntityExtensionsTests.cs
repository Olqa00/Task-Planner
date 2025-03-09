namespace Planner.Infrastructure.UnitTests.Extensions;

using Planner.Domain.Entities;
using Planner.Infrastructure.Extensions;
using Planner.Infrastructure.Models;

[TestClass]
public sealed class TaskEntityExtensionsTests
{
    private const string NEW_TITLE = "new title";
    private const string TITLE_1 = "title-1";
    private const string TITLE_2 = "title-2";
    private static readonly DateTime COMPETED_AT = new(year: 2025, month: 03, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 03, day: 04, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid ID_GUID_1 = Guid.NewGuid();
    private static readonly Guid ID_GUID_2 = Guid.NewGuid();

    private static readonly TaskId TASK_ID_1 = new(ID_GUID_1);
    private static readonly TaskId TASK_ID_2 = new(ID_GUID_2);

    private static readonly TaskDbEntity TASK_DB_ENTITY_1 = new()
    {
        Id = ID_GUID_1,
        Title = TITLE_1,
        CompletedAt = COMPETED_AT,
        CreatedAt = CREATED_AT,
    };

    private static readonly TaskDbEntity TASK_DB_ENTITY_2 = new()
    {
        Id = ID_GUID_2,
        Title = TITLE_2,
        CompletedAt = COMPETED_AT,
        CreatedAt = CREATED_AT,
    };

    private static readonly IEnumerable<TaskDbEntity> TASK_DB_ENTITIES = new List<TaskDbEntity>
    {
        TASK_DB_ENTITY_1,
        TASK_DB_ENTITY_2,
    };

    private readonly TaskEntity taskEntity1;
    private readonly TaskEntity taskEntity2;

    public TaskEntityExtensionsTests()
    {
        this.taskEntity1 = new TaskEntity(TASK_ID_1, TITLE_1, CREATED_AT, COMPETED_AT);
        this.taskEntity2 = new TaskEntity(TASK_ID_2, TITLE_2, CREATED_AT, COMPETED_AT);
    }

    [TestMethod]
    public void ToDbEntities_Should_ReturnTaskDbEntities()
    {
        // Arrange
        var entities = new List<TaskEntity>
        {
            this.taskEntity1,
            this.taskEntity2,
        };

        // Act
        var result = entities.ToDbEntities();

        // Assert
        result.Should()
            .BeEquivalentTo(TASK_DB_ENTITIES)
            ;
    }
}
