namespace Shared.Types;

public class ThorException : System.Exception
{
    protected ThorException(string message) : base(message)
    {
    }
}