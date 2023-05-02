using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Shared.Attributes;
using Shared.CQRS.Command;

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

        public static IServiceCollection AddCQRS(this IServiceCollection services)
        {
            return services.AddCommands();
        }
    }
}