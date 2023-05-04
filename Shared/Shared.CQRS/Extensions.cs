using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Shared.Attributes;
using Shared.CQRS.Command;
using Shared.CQRS.Query;

namespace Shared.CQRS
{
    public static class Extensions
    {
        private static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.Scan(s =>
                s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>))
                        .WithoutAttribute(typeof(Decorator)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            return services;
        }
        
        private static IServiceCollection AddQueries(this IServiceCollection services)
        {
            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
            services.Scan(s =>
                s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>))
                        .WithoutAttribute(typeof(Decorator)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            return services;
        }

        public static IServiceCollection AddCQRS(this IServiceCollection services)
        {
            return 
                services.AddCommands()
                .AddQueries();
        }
    }
}