using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Identity.Extensions;

public static class IdentityServerExtensions
{
    internal static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityServer()
            .AddInMemoryClients(new List<Client>())
            .AddInMemoryIdentityResources(new List<IdentityResource>())
            .AddInMemoryApiResources(new List<ApiResource>())
            .AddInMemoryApiScopes(new List<ApiScope>())
            .AddTestUsers(new List<TestUser>())
            .AddDeveloperSigningCredential();
        return services;
    }
    
    internal static IApplicationBuilder UseIdentity(this IApplicationBuilder app)
    {
        app.UseIdentityServer();
        return app;
    }
}