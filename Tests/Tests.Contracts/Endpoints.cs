using System.Security.Cryptography.X509Certificates;

namespace Tests.Contracts
{
    /// <summary>
    /// How the endpoints are stored in configs for easier access
    /// </summary>
    public static class Endpoints
    {
        public const string Apis = "Apis";
        public const string GameApiAddress = $"{Apis}:GameApiAddress";
    }
}