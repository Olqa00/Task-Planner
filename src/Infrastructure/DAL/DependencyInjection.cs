namespace Planner.Infrastructure.DAL;

public static class DependencyInjection
{
    private const string OPTIONS_SECTION_NAME = "SqlServer";

    public static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(OPTIONS_SECTION_NAME).Get<SqlServerOptions>();
        options ??= new SqlServerOptions();
        services.AddSingleton(options);

        services.AddHostedService<DatabaseInitializer>();

        return services;
    }
}
