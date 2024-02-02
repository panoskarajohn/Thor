using System.Text.Json;
using Game.Infrastructure.RedisDto;
using Microsoft.Extensions.Logging;
using Shared.Redis;
using StackExchange.Redis;


namespace Game.Infrastructure;

internal class MatchRepository : IMatchRepository
{
    private readonly IDatabase _database;
    private readonly RedisOptions _options;
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<MatchRepository> _logger;

    public MatchRepository(RedisOptions options, IConnectionMultiplexer redis, ILogger<MatchRepository> logger)
    {
        _redis = redis;
        _logger = logger;
        _database = _redis.GetDatabase();
        _options = options;
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PlayerDto?> FindMatchAsync(PlayerDto player, CancellationToken cancellationToken = default)
    {
        var eloRange = new RedisValue[] { player.Elo - 200, player.Elo + 200 };
        
        var opponents = await _database
            .SortedSetRangeByScoreAsync(MatchMakingKeys.MatchmakingQueue, start: (double)eloRange[0], stop: (double)eloRange[1], take: 1);

        foreach (var opponentData in opponents)
        {
            var opponent = await ValidateAndReturnPlayer(opponentData, player);
            if (opponent != null) return opponent;
        }

        throw new RedisException("Player not found");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="redisValue"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    private async Task<PlayerDto?> ValidateAndReturnPlayer(RedisValue redisValue, PlayerDto player)
    {
        var opponent = JsonSerializer.Deserialize<PlayerDto>(redisValue);
        if (opponent is null || player.Id.Equals(opponent.Id))
        {
            return null;
        }
        
        var delete = await _database.SortedSetRemoveAsync(MatchMakingKeys.MatchmakingQueue, redisValue);
        return delete ? opponent : null;
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