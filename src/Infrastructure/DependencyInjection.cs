namespace Planner.Infrastructure;

using System.Reflection;
using Planner.Domain.Interfaces;
using Planner.Infrastructure.DAL;
using Planner.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddSqlServer(configuration);

        services.AddScoped<ITaskRepository, TaskRepository>();

        return services;
    }
}
