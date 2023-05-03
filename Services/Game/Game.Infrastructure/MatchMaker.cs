using System.Text.Json.Serialization;
using Game.Infrastructure.RedisDto;
using Newtonsoft.Json;
using Shared.Redis;
using StackExchange.Redis;


namespace Game.Infrastructure;

internal class MatchMaker : IMatchMaker
{
    private readonly IDatabase _database;
    private readonly RedisOptions _options;
    private readonly IConnectionMultiplexer _redis;

    public MatchMaker(RedisOptions options, IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
        _options = options;
        
    }

    public async Task<PlayerDto?> FindMatchAsync(PlayerDto player, CancellationToken cancellationToken = default)
    {
        Thread.Sleep(5000);
        
        var eloRange = new RedisValue[] { player.Elo - 150, player.Elo + 200 };
        
        var opponents = await _database.SortedSetRangeByScoreAsync(MatchMakingKeys.MatchmakingQueue, start: (double)eloRange[0], stop: (double)eloRange[1], take: 1);
        
        if (opponents.Length < 1)
        {
            await _database.SortedSetAddAsync(MatchMakingKeys.MatchmakingQueue,
                JsonConvert.SerializeObject(player), 
                player.Elo);
            return null;
        }
        
        var opponent = JsonConvert.DeserializeObject<PlayerDto>(opponents.First()!);
        if (player == opponent)
        {
            await _database.SortedSetRemoveAsync(MatchMakingKeys.MatchmakingQueue, opponents.First());
            throw new Exception("Player is already in the queue");
        }
        
        await _database.SortedSetRemoveAsync(MatchMakingKeys.MatchmakingQueue, opponents.First());
        return opponent;
    }
    
}