using Game.Infrastructure.RedisDto;

namespace Game.Infrastructure;

public interface IMatchMaker
{
    public Task<PlayerDto?> FindMatchAsync(PlayerDto player, CancellationToken cancellationToken = default);
}