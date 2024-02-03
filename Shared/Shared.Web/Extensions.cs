using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Exception;
using Shared.Extensions;
using Shared.Web.Context;
using Shared.Web.HttpExtensions;
using Shared.Web.Middleware;
using Shared.Web.Options;

namespace Shared.Web
{
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddContext(this IServiceCollection services)
        {
            services.AddSingleton<ContextAccessor>();
            services.AddTransient(sp => sp.GetRequiredService<ContextAccessor>().Context);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddErrorHandling();

            return services;
        }
        
        
        public static IServiceCollection AddErrorHandling(this IServiceCollection services)
            => services
                .AddScoped<ErrorHandlingMiddleware>()
                .AddExceptionMapping();
        
        public static IServiceCollection AddWeb(this IServiceCollection services, IConfiguration configuration)
        {
            var appOptions = configuration.GetOptions<AppOptions>("app");
            services = services
                .AddSingleton(appOptions)
                .AddHttpContextAccessor()
                .AddContext();

            var version = appOptions.DisplayVersion ? $" {appOptions.Version}" : string.Empty;
            Console.WriteLine(Figgle.FiggleFonts.Doom.Render($"{appOptions.Name} v.{version}"));
        
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeb(this IApplicationBuilder app)
        {
            app.UseContext();
            app.UseCorrelationId();
            app.UseErrorHandling();
            return app;
        }
        
        static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
            => app.UseMiddleware<ErrorHandlingMiddleware>();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        private static IApplicationBuilder UseContext(this IApplicationBuilder app)
        {
            app.Use((ctx, next) =>
            {
                ctx.RequestServices.GetRequiredService<ContextAccessor>().Context = new Context.Context(ctx);
                return next();
            });
            return app;
        }
    }
}