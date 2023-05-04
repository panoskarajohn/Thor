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

        return _httpClient.PostAsJsonAsync("/match", request);
    }
    
    public async Task<long> GetQueueLength()
    {
        var response = await _httpClient.GetAsync("/match/queue");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.Content.ReadFromJsonAsync<long>();
    }
    
    public async Task<bool> CleanQueue()
    {
        var response = await _httpClient.DeleteAsync("/match/queue/clean");
        return response.StatusCode == HttpStatusCode.OK;
    }
}