namespace Shared.Redis.RateLimiter;

internal class RateLimiterKeys
{
    public const  string RateLimiter = "ratelimiter";
    public static string RateLimiterKey(string instance, string ipAddress) => $"{RateLimiter}:{instance}:{ipAddress}";
}