using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CodeYield.Mediator
{
    /// <summary>
    /// Extension methods for registering the mediator, handlers, and pipeline behaviors
    /// in the DI container.
    /// </summary>
    public static class MediatorServiceExtensions
    {
        /// <summary>
        /// Registers <see cref="IMediator"/> and scans <paramref name="assemblies"/> for handlers.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="assemblies">Assemblies to scan for handler implementations.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddMediator(this IServiceCollection services, params System.Reflection.Assembly[] assemblies)
        {
            return services.AddMediator(true, assemblies);
        }

        /// <summary>
        /// Registers <see cref="IMediator"/> and scans <paramref name="assemblies"/> for handlers.
        /// When <paramref name="registerBehaviors"/> is true, the default pipeline behaviors
        /// (logging, performance, validation) are also registered.
        /// </summary>
        public static IServiceCollection AddMediator(
            this IServiceCollection services,
            bool registerBehaviors,
            params System.Reflection.Assembly[] assemblies)
        {
            services.TryAddSingleton<IMediator, Mediator>();

            if (assemblies.Length > 0)
            {
                services.AddHandlers(assemblies);
            }

            if (registerBehaviors)
            {
                services.AddDefaultBehaviors();
            }

            return services;
        }

        /// <summary>
        /// Registers all three default pipeline behaviors:
        /// <see cref="LoggingBehavior{TRequest, TResponse}"/>,
        /// <see cref="PerformanceBehavior{TRequest, TResponse}"/>,
        /// and <see cref="ValidationBehavior{TRequest, TResponse}"/>.
        /// </summary>
        public static IServiceCollection AddDefaultBehaviors(this IServiceCollection services)
        {
            services.AddLoggingBehavior();
            services.AddPerformanceBehavior();
            services.AddValidationBehavior();
            return services;
        }

        /// <summary>Registers <see cref="LoggingBehavior{TRequest, TResponse}"/>.</summary>
        public static IServiceCollection AddLoggingBehavior(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            return services;
        }

        /// <summary>Registers <see cref="PerformanceBehavior{TRequest, TResponse}"/> with the default 500ms threshold.</summary>
        public static IServiceCollection AddPerformanceBehavior(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            return services;
        }

        /// <summary>Registers <see cref="PerformanceBehavior{TRequest, TResponse}"/> with a custom threshold.</summary>
        public static IServiceCollection AddPerformanceBehavior(this IServiceCollection services, long thresholdMs)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>),
                sp => ActivatorUtilities.CreateInstance<PerformanceBehavior<object, object>>(sp, thresholdMs));
            return services;
        }

        /// <summary>Registers <see cref="ValidationBehavior{TRequest, TResponse}"/>.</summary>
        public static IServiceCollection AddValidationBehavior(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }

        /// <summary>
        /// Scans the specified assemblies and registers all handler implementations:
        /// <see cref="ICommandHandler{TCommand}"/>, <see cref="ICommandHandler{TCommand, TResponse}"/>,
        /// <see cref="IQueryHandler{TQuery, TResponse}"/>, and <see cref="IDomainEventHandler{TEvent}"/>.
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
