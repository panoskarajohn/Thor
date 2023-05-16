using Microsoft.Extensions.DependencyInjection;

namespace Game.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddGameInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IMatchMaker, MatchMaker>();
        services.AddScoped<ILock, Lock>();
        return services;
    }
}