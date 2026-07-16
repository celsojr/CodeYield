using CodeYield.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CodeYield.Mediator
{
    /// <summary>
    /// In-process mediator that resolves handlers from the DI container and invokes them.
    /// Commands and queries are dispatched to a single handler; domain events are published
    /// to all registered handlers.
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>Initializes a new instance of <see cref="Mediator"/>.</summary>
        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public async Task SendAsync(ICommand command, CancellationToken ct = default)
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);
            await handler.HandleAsync((dynamic)command, ct);
        }

        /// <inheritdoc />
        public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct = default)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);
            return await handler.HandleAsync((dynamic)command, ct);
        }

        /// <inheritdoc />
        public async Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct = default)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);
            return await handler.HandleAsync((dynamic)query, ct);
        }

        /// <inheritdoc />
        public async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken ct = default) where TEvent : IDomainEvent
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(typeof(TEvent));
            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                if (handler is not null)
                    await ((dynamic)handler).HandleAsync(domainEvent, ct);
            }
        }
    }
}
