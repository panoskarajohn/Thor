using System.Net;
using System.Net.Http.Json;
using Tests.Contracts;
using Thor.Automation.Helpers;
using Thor.Automation.Services;

namespace Thor.Automation.StepDefinitions.MatchMakeSteps;

[Binding]
public class MatchMakeStepDefinitions
{
    private static Player _player = null;
    private static HttpResponseMessage _responseMessage;
    
    private readonly IMatchMakeRepository _repository;
    
    public MatchMakeStepDefinitions(IMatchMakeRepository repository)
    {
        _repository = repository;
    }
    
    [Given(@"a clean queue")]
    public async Task GivenACleanQueue()
    {
        await _repository.CleanQueue();
    }
    
    [Given(@"a player with elo (.*)")]
    public void GivenAPlayer(int elo)
    {
        _player = Helper.CreatePlayer(elo);
    }


    [When(@"the player searches for a match on the queue")]
    public async Task WhenThePlayerSearchesForAMatch()
    {
        var player = _player;
        var response = await _repository.MatchMake(player);
        _responseMessage = response;
    }
    
    [Then(@"the player should receive a (.*) status code")]
    public void ThenThePlayerShouldReceiveAStatusCode(int statusCode)
    {
        ((int) _responseMessage.StatusCode).Should().Be(statusCode);
    }

    [Then(@"queue should be empty")]
    public async Task ThenQueueShouldBeEmpty()
    {
        var queueLength = await _repository.GetQueueLength();
        queueLength.Should().Be(0);
    }
    
    [Then(@"queue should not be empty")]
    public async Task ThenQueueShouldNotBeEmpty()
    {
        var queueLength = await _repository.GetQueueLength();
        queueLength.Should().NotBe(0);
    }
}