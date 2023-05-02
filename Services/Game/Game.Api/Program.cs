using Game.Application.Commands;
using Shared.Community.Web;
using Shared.CQRS.Command;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var host = builder.Host;

services.AddWebCommon(configuration);
host.UseWebCommon(configuration);

var app = builder.Build();

app.UseWebCommon(configuration);

app.MapGet("/ping", () => "pong");
app.MapPost("/match",async (MatchmakeCommand command, ICommandDispatcher dispatcher) =>
{
    var response = await dispatcher.SendAsync(command);
    return response;
});

app.Run();