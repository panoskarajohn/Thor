using System.Security.AccessControl;
using StackExchange.Redis;

namespace Shared.Redis;

public class RedisLock : IDisposable
{
    private readonly IDatabase _database;
    private readonly string _lockKey;
    private readonly string _lockValue;
    private bool _isDisposed = false;


    public RedisLock(IDatabase redisDb, string lockKey, string lockValue, TimeSpan expirationTime)
    {
        _database = redisDb;
        _lockKey = lockKey;
        _lockValue = lockValue;
        if (!_database.StringSet(_lockKey, _lockValue, expirationTime, When.NotExists))
        {
            throw new LockException();
        }
    }
    
    public void Dispose()
    {
        if (!_isDisposed)
        {
            Release();
            _isDisposed = true;
        }
    }
    
    public void Release()
    {
        if (!_isDisposed)
        {
            _database.LockRelease(_lockKey, _lockValue);
            _isDisposed = true;
        }
    }
}