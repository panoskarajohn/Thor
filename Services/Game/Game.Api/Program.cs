using Game.Application.Commands;
using Game.Application.Commands.Response;
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
app.MapPost("/match",async (MatchmakeCommand command, ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var response = await dispatcher.SendAsync(command, cancellationToken);
    if (response is null)
        return Results.NotFound();
    return Results.Ok(response);
});

app.Run();