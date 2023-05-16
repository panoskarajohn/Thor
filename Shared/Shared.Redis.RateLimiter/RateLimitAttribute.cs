namespace Shared.Redis.RateLimiter;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RateLimitAttribute : Attribute
{
    
}