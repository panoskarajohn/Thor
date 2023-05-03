namespace Thor.Automation.Helpers;

public record Player(string Id, int Elo);

public class Helper
{
    public static Player CreatePlayer() => new Player(Guid.NewGuid().ToString(), GetRandomNumberFrom800to2000());

    private static int GetRandomNumberFrom800to2000() =>
        new Random().Next(800, 2000);
}
