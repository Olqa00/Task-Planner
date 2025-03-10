namespace Planner.IntegrationTests.Services;

using Planner.Application;
using Planner.Application.CommandHandlers;
using Planner.Application.Commands;
using Planner.Domain.Interfaces;
using Planner.Domain.Types;
using Planner.Infrastructure;
using Planner.Infrastructure.Services;

[TestClass]
public sealed class UpdateTaskTests
{
    private const string NEW_TITLE = "new task title";
    private const string TITLE = "task title";
    private static readonly DateTime COMPLETED_AT = new(year: 2025, month: 04, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 03, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private readonly IMediator mediator;
    private readonly ITaskRepository repository;

    public UpdateTaskTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: true)
            .Build();

        var serviceCollection = new ServiceCollection();
        var repositoryLogger = new NullLogger<TaskRepository>();
        var addHandlerLogger = new NullLogger<AddTaskHandler>();
        var updateHandlerLogger = new NullLogger<UpdateTaskHandler>();
        serviceCollection.AddApplication();
        serviceCollection.AddInfrastructure(configuration);
        serviceCollection.AddSingleton<ILogger<TaskRepository>>(repositoryLogger);
        serviceCollection.AddSingleton<ILogger<AddTaskHandler>>(addHandlerLogger);
        serviceCollection.AddSingleton<ILogger<UpdateTaskHandler>>(updateHandlerLogger);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        this.mediator = serviceProvider.GetRequiredService<IMediator>();
        this.repository = serviceProvider.GetRequiredService<ITaskRepository>();
    }

    [TestMethod]
    public async Task UpdateTask_WithValidData_ShouldReturnTask()
    {
        // Arrange
        var id = Guid.NewGuid();
        var taskId = new TaskId(id);

        var addCommand = new AddTask
        {
            CreatedAt = CREATED_AT,
            Id = id,
            Title = TITLE,
        };

        var updateCommand = new UpdateTask
        {
            CompletedAt = COMPLETED_AT,
            Id = id,
            Title = NEW_TITLE,
        };

        await this.mediator.Send(addCommand, CancellationToken.None);
        await Task.Delay(millisecondsDelay: 100, CancellationToken.None);

        // Act
        await this.mediator.Send(updateCommand, CancellationToken.None);
        await Task.Delay(millisecondsDelay: 100, CancellationToken.None);

        // Assert
        var result = await this.repository.GetByIdAsync(taskId);

        result.Should()
            .NotBeNull()
            ;

        result.Id.Should()
            .BeEquivalentTo(taskId)
            ;

        result.Title.Should()
            .BeEquivalentTo(NEW_TITLE)
            ;

        result.CompletedAt.Should()
            .Be(COMPLETED_AT)
            ;
    }
}
