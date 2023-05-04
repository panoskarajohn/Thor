namespace Thor.Automation.Helpers;

public record Player(string Id, int Elo);

public class Helper
{
    public static Player CreatePlayer(int elo) => new Player(Guid.NewGuid().ToString(), elo);
    
}
