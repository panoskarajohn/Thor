namespace Shared.CQRS.Command;

public interface ICommandDispatcher
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    Task<TResult> SendAsync<T, TResult>(T command, CancellationToken cancellationToken = default) where T : class, ICommand<TResult>;
}