using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Shared.Redis
{
    public static class Extensions
    {
        private const string RedisSection = "redis";
        private const int MaxRetries = 3;
        private const int RetryDelay = 100;
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetRequiredSection(RedisSection);
            var options = new RedisOptions();
            section.Bind(options);
            services.Configure<RedisOptions>(section);
            services.AddSingleton(options);
            
            services.AddSingleton<IConnectionMultiplexer>(provider =>
            {
                var logger = provider.GetService<ILogger<ConnectionMultiplexer>>();
                var connection = GetRedisConnection(options.ConnectionString, logger);
                
                return connection;
            });
            
            services.AddHealthChecks().AddCheck<RedisHealthCheck>("RedisConnectionCheck");
            

            return services;
        }
        
        private static ConnectionMultiplexer GetRedisConnection(string connectionString, ILogger<ConnectionMultiplexer> logger)
        {
            int retryCount = 0;
            while (retryCount < MaxRetries)
            {
                try
                {
                    return ConnectionMultiplexer.Connect(connectionString);
                }
                catch (RedisConnectionException e)
                {
                    retryCount++;
                    logger.LogWarning(
                        $"Failed to connect to Redis. Retrying in {RetryDelay / 1000} seconds. Retry count: {retryCount}");
                    Thread.Sleep(RetryDelay);
                }
            }
            
            throw new Exception("Failed to connect to Redis");
        }
    }
}