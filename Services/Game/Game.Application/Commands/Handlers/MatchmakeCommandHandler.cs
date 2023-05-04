using Game.Application.Commands.Response;
using Game.Infrastructure;
using Game.Infrastructure.RedisDto;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Shared.CQRS.Command;
using Shared.Redis;
using StackExchange.Redis;

namespace Game.Application.Commands.Handlers;

public class MatchMakeCommandHandler : ICommandHandler<MatchmakeCommand, MatchmakeResponse>
{
    private readonly ILogger<MatchMakeCommandHandler> _logger;
    private readonly IMatchMaker _matchMaker;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly ILock _lock;
    
    public MatchMakeCommandHandler(ILogger<MatchMakeCommandHandler> logger, IMatchMaker matchMaker, ILock @lock)
    {
        _logger = logger;
        _matchMaker = matchMaker;
        _lock = @lock;
        _retryPolicy = Policy
            .Handle<Exception>()//TODO: add specific exception to avoid retrying on circuit breaker
            .WaitAndRetryAsync(3, retryAttempt => {
                    _logger.LogWarning("Retrying to find match for player attempt {retryAttempt}", retryAttempt);
                    var timeToWait = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                    return timeToWait;
                }
            );
    }

    public async Task<MatchmakeResponse> HandleAsync(MatchmakeCommand command, CancellationToken cancellationToken = default)
    {
        var playerDto = new PlayerDto(command.PlayerId, command.Elo);
        using var playerLock = _lock.AcquireLock(playerDto.Id);
        _logger.LogInformation("Finding match for player {playerId}", playerDto.Id);
        var opponent = await _retryPolicy.ExecuteAsync(async () => await _matchMaker.FindMatchAsync(playerDto, cancellationToken));
        if (opponent is null) 
            return null;
        return new MatchmakeResponse() {PlayerMatchedId = opponent.Id, PlayerMatchedElo = opponent.Elo, MatchId = Guid.NewGuid().ToString()};
    }
}