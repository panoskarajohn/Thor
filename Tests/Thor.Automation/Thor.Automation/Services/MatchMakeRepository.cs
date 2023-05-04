using System.Net;
using System.Net.Http.Json;
using Tests.Contracts;
using Thor.Automation.Helpers;

namespace Thor.Automation.Services;

public class MatchMakeRepository : IMatchMakeRepository
{
    private readonly HttpClient _httpClient;

    public MatchMakeRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<HttpResponseMessage> MatchMake(Player player)
    {
        var request = new MatchMakeRequest
        {
            PlayerId = player.Id,
            Elo = player.Elo
        };

        var response = _httpClient.PostAsJsonAsync("/match", request);
        return response;
    }
}