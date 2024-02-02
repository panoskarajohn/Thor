using Game.Application.Commands;
using Game.Application.Queries;
using Game.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Shared.Community.Web;
using Shared.CQRS.Command;
using Shared.CQRS.Query;
using Shared.Redis;
using Shared.Redis.RateLimiter;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var host = builder.Host;
services.AddWebCommon(configuration);

host.UseWebCommon(configuration);

services.AddGameInfrastructure();
services.AddRedis(configuration);
services.AddRedisRateLimiter(configuration);

var app = builder.Build();

app.UseWebCommon(configuration);
app.UseRedisRateLimiter();

app.UseHealthChecks("/healthcheck", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

var rateLimitAttribute = new RateLimitAttribute();
app.MapGet("/ping", () => "pong")
    .WithMetadata(rateLimitAttribute);

app.MapPost("/match",async (MatchmakeCommand command, ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var response = await dispatcher.SendAsync(command, cancellationToken);
    if (response is null)
        return Results.NotFound();
    return Results.Ok(response);
}).WithMetadata(rateLimitAttribute);

app.MapGet("/match/queue", async (IQueryDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var query = new QueueQuery();
    var response = await dispatcher.QueryAsync(query, cancellationToken);
    return Results.Ok(response.Count);
}).WithMetadata(rateLimitAttribute);

app.MapDelete("/match/queue/clean", async (ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var command = new MatchMakeCleanCommand();
    var response = await dispatcher.SendAsync(command, cancellationToken);

    if(!response)
        return Results.StatusCode(500);

    return Results.NoContent();
}).WithMetadata(rateLimitAttribute);

app.Run();