namespace Game.Contracts.Requests
{
    public class MatchMakeRequest
    {
        public string PlayerId  { get; set; }
        public int Elo { get; set; }
    }
}