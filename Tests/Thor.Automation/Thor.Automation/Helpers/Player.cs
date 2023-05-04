namespace Thor.Automation.Helpers;

public record Player(string Id, int Elo);

public class Helper
{
    public static Player Create1200Player() => new Player(Guid.NewGuid().ToString(), 1200);
    
}
