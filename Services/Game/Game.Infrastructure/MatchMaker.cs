using System.Text.Json;
using Game.Infrastructure.RedisDto;
using Microsoft.Extensions.Logging;
using Shared.Redis;
using StackExchange.Redis;


namespace Game.Infrastructure;

internal class MatchMaker : IMatchMaker
{
    private readonly IDatabase _database;
    private readonly RedisOptions _options;
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<MatchMaker> _logger;

    public MatchMaker(RedisOptions options, IConnectionMultiplexer redis, ILogger<MatchMaker> logger)
    {
        _redis = redis;
        _logger = logger;
        _database = _redis.GetDatabase();
        _options = options;
        
    }

    public async Task<PlayerDto?> FindMatchAsync(PlayerDto player, CancellationToken cancellationToken = default)
    {
        var eloRange = new RedisValue[] { player.Elo - 200, player.Elo + 200 };
        
        var opponents = await _database.SortedSetRangeByScoreAsync(MatchMakingKeys.MatchmakingQueue, start: (double)eloRange[0], stop: (double)eloRange[1], take: 1);
        
        if (opponents.Length < 1)
        {
            _logger.LogInformation("No opponents found for player {playerId}", player.Id);
            await _database.SortedSetAddAsync(MatchMakingKeys.MatchmakingQueue,
                JsonSerializer.Serialize(player), 
                player.Elo);
            return null;
        }
        
        var opponent = JsonSerializer.Deserialize<PlayerDto>(opponents.First()!);
        if (player.Equals(opponent))
        {
            _logger.LogWarning("Player {playerId} is already in the queue and matched with himself", player.Id);
            throw new Exception("Player is already in the queue");
        }
        
        await _database.SortedSetRemoveAsync(MatchMakingKeys.MatchmakingQueue, opponents.First());
        _logger.LogInformation("Found opponent {opponentId}:{opponentElo} for player {playerId}:{playerElo}", opponent.Id, opponent.Elo, player.Id, player.Elo);
        return opponent;
    }

    /// <summary>
    /// Gets the queue length
    /// </summary>
    /// <returns></returns>
    public Task<long> GetQueueLength()
    {
        return _database.SortedSetLengthAsync(MatchMakingKeys.MatchmakingQueue);
    }

    /// <summary>
    /// Cleans the queue from all players
    /// </summary>
    /// <returns></returns>
    public Task<bool> CleanQueue()
    {
        return _database.KeyDeleteAsync(MatchMakingKeys.MatchmakingQueue);
    }
}