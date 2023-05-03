using Shared.Redis;
using StackExchange.Redis;

namespace Game.Infrastructure;

public interface ILock
{
    RedisLock AcquireLock(string playerId);
}

internal class Lock : ILock
{
    private readonly IDatabase _database;
    private readonly RedisOptions _options;
    private readonly IConnectionMultiplexer _redis;
    public Lock(RedisOptions options, IConnectionMultiplexer redis)
    {
        _options = options;
        _redis = redis;
        _database = _redis.GetDatabase();
    }
    public RedisLock AcquireLock(string playerId)
    {
        return new RedisLock(_database,
            MatchMakingKeys.MatchmakingLockPlayer(playerId),
            playerId, _options.LockExpirationTime);
    }
}