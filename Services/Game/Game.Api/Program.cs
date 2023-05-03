using Game.Application.Commands;
using Game.Contracts.Requests;
using Game.Infrastructure;
using Shared.Community.Web;
using Shared.CQRS.Command;
using Shared.Redis;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var host = builder.Host;

services.AddWebCommon(configuration);
host.UseWebCommon(configuration);

services.AddGameInfrastructure();
services.AddRedis(configuration);

var app = builder.Build();

app.UseWebCommon(configuration);

app.MapGet("/ping", () => "pong");
app.MapPost("/match",async (MatchMakeRequest request, ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var matchMakeCommand = new MatchmakeCommand()
    {
        PlayerId = request.PlayerId,
        Elo = request.Elo
    };
    var response = await dispatcher.SendAsync(matchMakeCommand, cancellationToken);
    if (response is null)
        return Results.NotFound();
    return Results.Ok(response);
});

app.Run();