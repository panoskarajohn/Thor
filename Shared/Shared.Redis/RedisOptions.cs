namespace Shared.Redis;

public class RedisOptions
{
    public string ConnectionString { get; set; }
    public TimeSpan LockExpirationTime { get; set; } = TimeSpan.FromSeconds(10);
}