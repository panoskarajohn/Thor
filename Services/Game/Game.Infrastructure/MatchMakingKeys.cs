namespace Game.Infrastructure;

public static class MatchMakingKeys
{
    public const string MatchmakingQueue = "matchmaking:queue";
    public const string MatchmakingLock = "matchmaking:lock";
    public static string MatchmakingLockPlayer(string playerId) => $"{MatchmakingLock}:{playerId}";
}