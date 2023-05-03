using Microsoft.Extensions.DependencyInjection;

namespace Game.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddGameInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IMatchMaker, MatchMaker>();
        services.AddSingleton<ILock, Lock>();
        return services;
    }
}