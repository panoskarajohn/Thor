using Game.Application.Commands.Response;
using Microsoft.Extensions.Logging;
using Shared.CQRS.Command;

namespace Game.Application.Commands.Handlers;

public class MatchmakeCommandHandler : ICommandHandler<MatchmakeCommand, MatchmakeResponse>
{
    private readonly ILogger<MatchmakeCommandHandler> _logger;

    public MatchmakeCommandHandler(ILogger<MatchmakeCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task<MatchmakeResponse> HandleAsync(MatchmakeCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("MatchmakeCommandHandler.HandleAsync");

        return Task.FromResult(new MatchmakeResponse() { MatchId = Guid.NewGuid().ToString(), PlayerMatchedId = Guid.NewGuid().ToString(), PlayerMatchedElo = 1200});
    }
}