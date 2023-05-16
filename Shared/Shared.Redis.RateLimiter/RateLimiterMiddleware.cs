using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Shared.Redis.RateLimiter;

internal class RateLimiterMiddleware : IMiddleware
{
    private readonly ILogger<RateLimiterMiddleware> _logger;
    private readonly EndpointDataSource _endPoint;
    private readonly IRateLimiter _rateLimiter;
    private readonly RateLimiterOptions _options;

    public RateLimiterMiddleware(ILogger<RateLimiterMiddleware> logger, EndpointDataSource endPoint, IRateLimiter rateLimiter, RateLimiterOptions options)
    {
        _logger = logger;
        _endPoint = endPoint;
        _rateLimiter = rateLimiter;
        _options = options;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      var endpoint = _endPoint.Endpoints.FirstOrDefault(x => x is RouteEndpoint routeEndpoint && routeEndpoint.RoutePattern.RawText == context.Request.Path);
      var attribute = endpoint?.Metadata.GetMetadata<RateLimitAttribute>();
      if (attribute is null)
      {
        await next(context);
        return;
      }
      
      var ipAddress = context.Connection.RemoteIpAddress?.ToString();

      if (string.IsNullOrEmpty(ipAddress))
      {
            _logger.LogWarning("Could not get IP address from request");
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            return;
      }

      var isAllowed = await _rateLimiter.IsRequestAllowedAsync(_options.Instance, ipAddress, _options.MaxRequests, _options.Expire);

      if (!isAllowed)
      {
          _logger.LogWarning("Too many requests from {ipAddress}", ipAddress);
          context.Response.StatusCode = (int) HttpStatusCode.TooManyRequests;
          return;
      }

      await next(context);
    }
}