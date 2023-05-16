namespace Shared.Redis.RateLimiter;

internal interface IRateLimiter
{
    Task<bool> IsRequestAllowedAsync(string instance, string ipAddress, int maxRequests, TimeSpan? expire);
}