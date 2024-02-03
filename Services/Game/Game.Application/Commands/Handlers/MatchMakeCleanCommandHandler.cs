using Game.Infrastructure;
using Microsoft.Extensions.Logging;
using Shared.CQRS.Command;

namespace Game.Application.Commands.Handlers;

public class MatchMakeCleanCommandHandler : ICommandHandler<MatchMakeCleanCommand, bool>
{
    private readonly IMatchRepository _matchRepository;
    private readonly ILogger<MatchMakeCleanCommandHandler> _logger;

    public MatchMakeCleanCommandHandler(IMatchRepository matchRepository, ILogger<MatchMakeCleanCommandHandler> logger)
    {
        _matchRepository = matchRepository;
        _logger = logger;
    }

    public Task<bool> HandleAsync(MatchMakeCleanCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("Deleting all players from the queue");
        return _matchRepository.CleanQueue();
    }
}