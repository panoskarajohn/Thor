using Microsoft.Extensions.DependencyInjection;

namespace Shared.Exception;

public static class Extensions
{
    public static IServiceCollection AddExceptionMapping(this IServiceCollection services)
        => services
            .AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>()
            .AddSingleton<IExceptionCompositionRoot, ExceptionCompositionRoot>();
}