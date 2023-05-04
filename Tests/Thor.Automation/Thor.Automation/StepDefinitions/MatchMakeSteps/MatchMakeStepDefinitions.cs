using System.Net;
using System.Net.Http.Json;
using Tests.Contracts;
using Thor.Automation.Helpers;

namespace Thor.Automation.StepDefinitions.MatchMakeSteps;

[Binding]
public class MatchMakeStepDefinitions
{
    private static Player _player = null;
    private static HttpStatusCode _statusCode;
    
    private readonly HttpClient _httpClient;
    
    public MatchMakeStepDefinitions(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    [Given(@"a player")]
    public void GivenAPlayer()
    {
        _player = Helper.Create1200Player();
    }


    [When(@"the player searches for a match on the queue")]
    public void WhenThePlayerSearchesForAMatch()
    {
        var player = _player;
        var request = new MatchMakeRequest
        {
            PlayerId = player.Id,
            Elo = player.Elo
        };

        var response = _httpClient.PostAsJsonAsync("/match", request).Result;
        _statusCode = response.StatusCode;
    }
    
    [Then(@"the player should receive a (.*) status code")]
    public void ThenThePlayerShouldReceiveAStatusCode(int statusCode)
    {
        ((int) _statusCode).Should().Be(statusCode);
    }
}