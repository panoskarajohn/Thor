using Microsoft.Extensions.DependencyInjection;
using Shared.Types;

namespace Shared.Exception;

internal sealed class ExceptionCompositionRoot : IExceptionCompositionRoot
{
    private readonly IServiceProvider _serviceProvider;

    public ExceptionCompositionRoot(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
        
    public ExceptionResponse Map(System.Exception exception)
    {
        using var scope = _serviceProvider.CreateScope();
        var mappers = scope.ServiceProvider.GetServices<IExceptionToResponseMapper>().ToArray();
        var nonDefaultMappers = mappers.Where(x => x is not ExceptionToResponseMapper);
        var result = nonDefaultMappers
            .Select(x => x.Map(exception))
            .SingleOrDefault(x => x is not null);

        if (result is not null)
        {
            return result;
        }
        
        var defaultMapper = mappers.SingleOrDefault(x => x is ExceptionToResponseMapper);

        if(exception is ThorException)
            return defaultMapper?.Map(exception);

        return defaultMapper?.Map(exception);
    }
}