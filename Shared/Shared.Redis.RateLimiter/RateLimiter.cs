using System.Net;
using System.Text.Json;
using StackExchange.Redis;

namespace Shared.Redis.RateLimiter;

internal class RateLimiter : IRateLimiter
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public RateLimiter(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
    }

    public async Task<bool> IsRequestAllowedAsync(string instance, string ipAddress, int maxRequests, TimeSpan? expire)
    {
        var key = RateLimiterKeys.RateLimiterKey(instance ,ipAddress);
        expire ??= TimeSpan.FromHours(1);

        var counter = await _database.StringGetAsync(key);
        if (!counter.HasValue)
        {
            await _database.StringSetAsync(key, 0, expire);
        }

        var incrementedValue = await _database.StringIncrementAsync(key);

        if (incrementedValue > maxRequests)
        {
            return false;
        }

        return true;
    }
}

