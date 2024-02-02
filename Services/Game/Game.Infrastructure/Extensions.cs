using Microsoft.Extensions.DependencyInjection;

namespace Game.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddGameInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<ILock, Lock>();
        return services;
    }
}