namespace Shared.Redis.RateLimiter;

internal class RateLimiterOptions
{
    public int MaxRequests { get; set; }
    public TimeSpan? Expire { get; set; }
    public string Instance { get; set; }
}