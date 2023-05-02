using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.CQRS;
using Shared.Logging;
using Shared.Web;

namespace Shared.Community.Web
{
    public static class Extensions
    {
        /// <summary>
        /// Adds useful extensions for web such as
        /// logging, error middleware, CQRSr
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebCommon(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWeb(configuration);
            services.AddCQRS();
            
            return services;
        }

        public static IApplicationBuilder UseWebCommon(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseWeb();
            app.UseSerilogLogging();
            return app;
        }
        
        public static IHostBuilder UseWebCommon(this IHostBuilder host, IConfiguration configuration)
        {
            host.UseSerilogLogging();
            return host;
        }
    }
}