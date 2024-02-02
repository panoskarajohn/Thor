using Identity.Extensions;
using IdentityServer4;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddIdentity();

var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.Run();