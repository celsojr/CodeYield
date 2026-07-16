using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CodeYield.EventBus
{
    /// <summary>
    /// Extension methods for registering the event bus and its background service in the DI container.
    /// </summary>
    public static class EventBusServiceExtensions
    {
        /// <summary>
        /// Registers a singleton <see cref="IEventBus"/> backed by a channel, an
        /// <see cref="IHttpClientFactory"/>, and the <see cref="EventBusBackgroundService"/>.
        /// </summary>
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            var eventBus = new EventBus();
            services.TryAddSingleton<IEventBus>(eventBus);
            services.TryAddSingleton(eventBus);
            services.AddHostedService<EventBusBackgroundService>();
            return services;
        }
    }
}
