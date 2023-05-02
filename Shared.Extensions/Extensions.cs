using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions
{
    public static class Extensions
    {
        
            /// <summary>
            /// Binds option class to configuration section
            /// </summary>
            /// <param name="configuration"></param>
            /// <param name="sectionName"></param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
            {
                var options = new T();
                configuration.GetSection(sectionName).Bind(options);
                return options;
            }
    
            /// <summary>
            /// Will bind configuration section to the object
            /// </summary>
            /// <param name="section"></param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static T BindOptions<T>(this IConfigurationSection section) where T : new()
            {
                var options = new T();
                section.Bind(options);
                return options;
            }
        }
}