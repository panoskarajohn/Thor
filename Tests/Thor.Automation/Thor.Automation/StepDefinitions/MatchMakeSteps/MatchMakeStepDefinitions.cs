using System.Net;
using System.Net.Http.Json;
using Game.Contracts.Requests;

namespace Thor.Automation.StepDefinitions.MatchMakeSteps;

[Binding]
public class MatchMakeStepDefinitions
{
    private readonly HttpClient _httpClient;
    public MatchMakeStepDefinitions(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [When(@"a player searches for a match")]
    public void WhenAPlayerSearchesForAMatch()
    {
        var player = Helpers.Helper.CreatePlayer();
        var request = new MatchMakeRequest
        {
            PlayerId = player.Id,
            Elo = player.Elo
        };

        var response = _httpClient.PostAsJsonAsync("api/matchmake", request).Result;
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

    }


}