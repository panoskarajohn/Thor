﻿namespace Shared.CQRS.Command;

public interface ICommandHandler<in TCommand, TResult> where TCommand : class, ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
