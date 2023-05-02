namespace Game.Application.Commands.Response;

public class MatchmakeResponse
{
    public string MatchId { get; set; }
    public string PlayerMatchedId { get; set; }
    public int PlayerMatchedElo { get; set; }
}