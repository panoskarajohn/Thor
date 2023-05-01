using Microsoft.Extensions.DependencyInjection;
using Shared.CQRS;

namespace Shared.Community.Web
{
    public static class Extensions
    {
        /// <summary>
        /// Adds useful extensions for web such as
        /// logging, error middleware, CQRS, global exception type, swagger
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebCommon(this IServiceCollection services)
        {
            services.AddCQRS();
            return services;
        }
    }
}