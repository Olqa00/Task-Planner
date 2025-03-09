namespace Planner.Infrastructure;

using Planner.Domain.Interfaces;
using Planner.Infrastructure.DAL;
using Planner.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer(configuration);

        services.AddScoped<ITaskRepository, TaskRepository>();

        return services;
    }
}
