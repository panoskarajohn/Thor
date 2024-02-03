using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Exception;
using Shared.Types;

namespace Shared.Web.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly IExceptionCompositionRoot _exceptionCompositionRoot;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(IExceptionCompositionRoot exceptionCompositionRoot,
        ILogger<ErrorHandlingMiddleware> logger)
    {
        _exceptionCompositionRoot = exceptionCompositionRoot;
        _logger = logger;
    }
        
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            _logger.LogError("Request timed out.");
            context.Response.StatusCode = (int) HttpStatusCode.RequestTimeout;
            await context.Response.WriteAsJsonAsync(new { error = "Request timed out." }, context.RequestAborted);
        }
        catch (ThorException exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleErrorAsync(context, exception);
        }
        catch (System.Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleErrorAsync(context, exception);
        }
    }

    private Task HandleErrorAsync(HttpContext context, System.Exception exception)
    {
        var errorResponse = _exceptionCompositionRoot.Map(exception);
        context.Response.StatusCode = (int) (errorResponse?.StatusCode ?? HttpStatusCode.InternalServerError);
        var response = errorResponse?.Response;
        if (response is null)
        {
            return Task.CompletedTask;
        }
            
        return context.Response.WriteAsJsonAsync(response);
    }
}

