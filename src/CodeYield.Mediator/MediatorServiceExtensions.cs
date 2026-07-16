using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CodeYield.Mediator
{
    /// <summary>
    /// Extension methods for registering the mediator and its handlers in the DI container.
    /// </summary>
    public static class MediatorServiceExtensions
    {
        /// <summary>
        /// Registers <see cref="IMediator"/> as a singleton and scans
        /// <paramref name="assemblies"/> for command, query, and domain event handlers.
        /// </summary>
        public static IServiceCollection AddMediator(this IServiceCollection services, params System.Reflection.Assembly[] assemblies)
        {
            services.TryAddSingleton<IMediator, Mediator>();

            if (assemblies.Length > 0)
            {
                services.AddHandlers(assemblies);
            }

            return services;
        }

        /// <summary>
        /// Scans the specified assemblies and registers all <see cref="ICommandHandler{TCommand}"/>,
        /// <see cref="ICommandHandler{TCommand, TResponse}"/>, <see cref="IQueryHandler{TQuery, TResponse}"/>,
        /// and <see cref="IDomainEventHandler{TEvent}"/> implementations.
        /// </summary>
        public static IServiceCollection AddHandlers(this IServiceCollection services, params System.Reflection.Assembly[] assemblies)
        {
            var handlerInterfaceTypes = new[]
            {
                typeof(ICommandHandler<>),
                typeof(ICommandHandler<,>),
                typeof(IQueryHandler<,>),
                typeof(IDomainEventHandler<>)
            };

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsAbstract || type.IsInterface) continue;

                    foreach (var @interface in type.GetInterfaces())
                    {
                        if (!@interface.IsGenericType) continue;

                        var genericDef = @interface.GetGenericTypeDefinition();
                        if (Array.IndexOf(handlerInterfaceTypes, genericDef) < 0) continue;

                        services.TryAddTransient(@interface, type);
                    }
                }
            }

            return services;
        }
    }
}
