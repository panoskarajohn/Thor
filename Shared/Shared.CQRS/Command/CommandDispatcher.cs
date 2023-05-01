using Microsoft.Extensions.DependencyInjection;

namespace Shared.CQRS.Command;

internal sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> SendAsync<T, TResult>(T command, CancellationToken cancellationToken = default) where T : class, ICommand<TResult>
    {
        using var scope = _serviceProvider.CreateScope();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);
        
        return await (Task<TResult>) handlerType
            .GetMethod(nameof(ICommandHandler<ICommand<TResult>, TResult>.HandleAsync))!
            .Invoke(handler, new object[] { command, cancellationToken })!;
    }
}