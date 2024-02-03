using Game.Application.Queries.Response;
using Game.Infrastructure;
using Microsoft.Extensions.Logging;
using Shared.CQRS.Query;

namespace Game.Application.Queries.Handlers;

public class QueueStateQueryHandler : IQueryHandler<QueueQuery, QueueStateResponse>
{
    private readonly IMatchRepository _matchRepository;
    private readonly ILogger<QueueStateQueryHandler> _logger;

    public QueueStateQueryHandler(IMatchRepository matchRepository, ILogger<QueueStateQueryHandler> logger)
    {
        _matchRepository = matchRepository;
        _logger = logger;
    }

    public async Task<QueueStateResponse> HandleAsync(QueueQuery query, CancellationToken cancellationToken = default)
    {
        var length = await _matchRepository.GetQueueLength();
        _logger.LogInformation("Queue length is {length}", length);
        return new QueueStateResponse
        {
            Count = length
        };
    }
}