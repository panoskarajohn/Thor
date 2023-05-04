using Game.Application.Commands;
using Game.Application.Commands.Response;
using Game.Application.Queries;
using Game.Infrastructure;
using Shared.Community.Web;
using Shared.CQRS.Command;
using Shared.CQRS.Query;
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
app.MapPost("/match",async (MatchmakeCommand command, ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var response = await dispatcher.SendAsync(command, cancellationToken);
    if (response is null)
        return Results.NotFound();
    return Results.Ok(response);
});

app.MapGet("match/queue", async (IQueryDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var query = new QueueQuery();
    var response = await dispatcher.QueryAsync(query, cancellationToken);
    return Results.Ok(response.Count);
});

app.MapDelete("match/queue/clean", async (ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var command = new MatchMakeCleanCommand();
    var response = await dispatcher.SendAsync(command, cancellationToken);
    return Results.NoContent();
});

app.Run();