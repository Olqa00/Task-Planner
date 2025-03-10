namespace Planner.IntegrationTests.Services;

using System.Reflection;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Planner.Application;
using Planner.Application.CommandHandlers;
using Planner.Application.Commands;
using Planner.Domain.Interfaces;
using Planner.Domain.Types;
using Planner.Infrastructure;
using Planner.Infrastructure.Services;

[TestClass]
public sealed class AddTaskTests
{
    private const string TITLE = "task title";
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 03, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private readonly IMediator mediator;
    private readonly ITaskRepository repository;

    public AddTaskTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: true)
            .Build();

        var serviceCollection = new ServiceCollection();
        var repositoryLogger = new NullLogger<TaskRepository>();
        var handlerLogger = new NullLogger<AddTaskHandler>();
        serviceCollection.AddApplication();
        serviceCollection.AddInfrastructure(configuration);
        serviceCollection.AddSingleton<ILogger<TaskRepository>>(repositoryLogger);
        serviceCollection.AddSingleton<ILogger<AddTaskHandler>>(handlerLogger);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        this.mediator = serviceProvider.GetRequiredService<IMediator>();
        this.repository = serviceProvider.GetRequiredService<ITaskRepository>();
    }

    [TestMethod]
    public async Task AddTask_WithValidData_ShouldReturnTask()
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

        // Act
        await this.mediator.Send(command, CancellationToken.None);
        await Task.Delay(millisecondsDelay: 100, CancellationToken.None);

        // Assert
        var taskEntity = await this.repository.GetByIdAsync(taskId);

        taskEntity.Should()
            .NotBeNull()
            ;
    }
}
