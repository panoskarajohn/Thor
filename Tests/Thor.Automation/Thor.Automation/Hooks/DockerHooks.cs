using System.Net;
using BoDi;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Configuration;
using Tests.Contracts;
using Thor.Automation.Services;

namespace Thor.Automation.Hooks;

[Binding]
public class DockerHooks
{
    private static ICompositeService _compositeService;
    private readonly IObjectContainer _container;

    public DockerHooks(IObjectContainer container)
    {
        _container = container;
    }

    [BeforeScenario()]
    public void AddHttpClients()
    {
        RegisterMatchClient();
    }
    
    private void RegisterMatchClient()
    {
        var config = LoadConfiguration();
        var confirmationUrl = config[Endpoints.GameApiAddress];
        var httpClient = new HttpClient()
        {
            BaseAddress = new Uri(confirmationUrl),
        };

        var repository = new MatchMakeRepository(httpClient);
        _container.RegisterInstanceAs<IMatchMakeRepository>(repository);
    }

    [BeforeTestRun]
    public static void DockerComposeUp()
    {
        var config = LoadConfiguration();
        var dockerComposeFileName = config["DockerComposeFileName"];
        var dockerComposeOverrideFileName = config["DockerComposeOverrideFileName"];
        var dockerComposePath = GetDockerComposeDirectory(dockerComposeFileName);
        var dockerComposeOverridePath = GetDockerComposeDirectory(dockerComposeOverrideFileName);
        var confirmationUrl = config[Endpoints.GameApiAddress];
        _compositeService = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(dockerComposePath)
            .FromFile(dockerComposeOverridePath)
            .RemoveOrphans()
            .WaitForHttp("eventapi", url: confirmationUrl, continuation: (response, _) =>
                response.Code != HttpStatusCode.OK ? 2000 : 0
            )
            .Build()
            .Start();
    }

    [AfterTestRun]
    public static void DockerComposeDown()
    {
        _compositeService.Stop();
        _compositeService.Dispose(); // This will delete the containers losing sql data.
    }

    private static IConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }

    private static string GetDockerComposeDirectory(string dockerComposeFileName)
    {
        var directory = Directory.GetCurrentDirectory();
        while (!Directory.EnumerateFiles(directory, "*.yml").Any(s => s.EndsWith(dockerComposeFileName)))
        {
            directory = directory.Substring(0, directory.LastIndexOf(Path.DirectorySeparatorChar));
        }
        return Path.Combine(directory, dockerComposeFileName);
    }
}