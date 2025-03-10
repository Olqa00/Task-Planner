namespace Planner.IntegrationTests.Services;

using Planner.Application;
using Planner.Application.CommandHandlers;
using Planner.Application.Commands;
using Planner.Domain.Interfaces;
using Planner.Domain.Types;
using Planner.Infrastructure;
using Planner.Infrastructure.QueryHandlers;
using Planner.Infrastructure.Services;

[TestClass]
public sealed class GetTaskTests
{
    private const string TITLE = "task title";
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 03, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private readonly IMediator mediator;
    private readonly ITaskRepository repository;

    public GetTaskTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: true)
            .Build();

        var serviceCollection = new ServiceCollection();
        var repositoryLogger = new NullLogger<TaskRepository>();
        var addHandlerLogger = new NullLogger<AddTaskHandler>();
        var getHandlerLogger = new NullLogger<GetTaskHandler>();
        serviceCollection.AddApplication();
        serviceCollection.AddInfrastructure(configuration);
        serviceCollection.AddSingleton<ILogger<TaskRepository>>(repositoryLogger);
        serviceCollection.AddSingleton<ILogger<AddTaskHandler>>(addHandlerLogger);
        serviceCollection.AddSingleton<ILogger<GetTaskHandler>>(getHandlerLogger);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        this.mediator = serviceProvider.GetRequiredService<IMediator>();
        this.repository = serviceProvider.GetRequiredService<ITaskRepository>();
    }

    [TestMethod]
    public async Task GetTask_WithValidData_ShouldReturnTask()
    {
        // Arrange
        var id = Guid.NewGuid();
        var taskId = new TaskId(id);

        var command = new AddTask
        {
            CreatedAt = CREATED_AT,
            Id = id,
            Title = TITLE,
        };

        await this.mediator.Send(command, CancellationToken.None);
        await Task.Delay(millisecondsDelay: 100, CancellationToken.None);

        // Act
        var taskEntity = await this.repository.GetByIdAsync(taskId);
        await Task.Delay(millisecondsDelay: 100, CancellationToken.None);

        // Assert
        taskEntity.Should()
            .NotBeNull()
            ;
    }
}
