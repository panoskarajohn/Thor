namespace Shared.Redis;

public class LockException : Exception
{
    public LockException(string message) : base(message) { }
}