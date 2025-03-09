namespace Planner.Infrastructure.DAL;

public static class DependencyInjection
{
    private const string OPTIONS_SECTION_NAME = "ConnectionStrings";

    public static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(OPTIONS_SECTION_NAME);
        services.Configure<SqlServerOptions>(section);

        services.AddHostedService<DatabaseInitializer>();

        return services;
    }
}
