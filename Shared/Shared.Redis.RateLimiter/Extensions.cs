using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Redis.RateLimiter;

public static class Extensions
{
    private const string RateLimiterSection = "rateLimiter";
    
    /// <summary>
    /// You should have already registered Redis in your DI container
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddRedisRateLimiter(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection(RateLimiterSection);
        var options = new RateLimiterOptions();
        section.Bind(options);
        services.Configure<RateLimiterOptions>(section);
        services.AddSingleton(options);

        services
            .AddScoped<RateLimiterMiddleware>()
            .AddSingleton<IRateLimiter, RateLimiter>();

        return services;
    }

    public static IApplicationBuilder UseRedisRateLimiter(this IApplicationBuilder app)
    {
        app.UseMiddleware<RateLimiterMiddleware>();
        return app;
    }

}