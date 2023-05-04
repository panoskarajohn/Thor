using Game.Infrastructure;
using Microsoft.Extensions.Logging;
using Shared.CQRS.Command;

namespace Game.Application.Commands.Handlers;

public class MatchMakeCleanCommandHandler : ICommandHandler<MatchMakeCleanCommand, bool>
{
    private readonly IMatchMaker _matchMaker;
    private readonly ILogger<MatchMakeCommandHandler> _logger;

    public MatchMakeCleanCommandHandler(IMatchMaker matchMaker, ILogger<MatchMakeCommandHandler> logger)
    {
        _matchMaker = matchMaker;
        _logger = logger;
    }

    public Task<bool> HandleAsync(MatchMakeCleanCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("Deleting all players from the queue");
        return _matchMaker.CleanQueue();
    }
}