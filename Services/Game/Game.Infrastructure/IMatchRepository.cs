using Game.Infrastructure.RedisDto;

namespace Game.Infrastructure;

public interface IMatchRepository
{
    public Task<PlayerDto?> FindMatchAsync(PlayerDto player, CancellationToken cancellationToken = default);
    Task<bool> AddPlayerToQueue(PlayerDto player);
    public Task<long> GetQueueLength();
    
    public Task<bool> CleanQueue();
}