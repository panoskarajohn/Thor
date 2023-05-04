using Thor.Automation.Helpers;

namespace Thor.Automation.Services;

public interface IMatchMakeRepository
{
    Task<HttpResponseMessage> MatchMake(Player player);
}