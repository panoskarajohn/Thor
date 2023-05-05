using Shared.Types;

namespace Shared.Redis;

public class LockException : ThorException
{
    public LockException() : base($"Player already in queue. Please wait for a match.")
    {
    }
}