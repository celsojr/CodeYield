using System.Reflection;
using CodeYield.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CodeYield.Mediator
{
    /// <summary>
    /// In-process mediator that resolves handlers from the DI container and invokes them
    /// through a chain of <see cref="IPipelineBehavior{TRequest, TResponse}"/> before reaching
    /// the final handler.
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        private static readonly MethodInfo BuildPipelineMethod =
            typeof(Mediator).GetMethod(nameof(BuildPipeline), BindingFlags.NonPublic | BindingFlags.Instance)!;

        /// <summary>Initializes a new instance of <see cref="Mediator"/>.</summary>
        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public async Task SendAsync(ICommand command, CancellationToken ct = default)
        {
            var requestType = command.GetType();
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(requestType);
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);
            await handler.HandleAsync((dynamic)command, ct);
        }

        /// <inheritdoc />
        public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct = default)
        {
            var requestType = command.GetType();
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);

            Func<Task<TResponse>> handlerFunc = () => handler.HandleAsync((dynamic)command, ct);
            var method = BuildPipelineMethod.MakeGenericMethod(requestType, typeof(TResponse));
            var pipeline = (Func<Task<TResponse>>)method.Invoke(this, new object[] { command, handlerFunc, ct })!;

            return await pipeline();
        }

        /// <inheritdoc />
        public async Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct = default)
        {
            var requestType = query.GetType();
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);

            Func<Task<TResponse>> handlerFunc = () => handler.HandleAsync((dynamic)query, ct);
            var method = BuildPipelineMethod.MakeGenericMethod(requestType, typeof(TResponse));
            var pipeline = (Func<Task<TResponse>>)method.Invoke(this, new object[] { query, handlerFunc, ct })!;

            return await pipeline();
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

        private Func<Task<TResponse>> BuildPipeline<TRequest, TResponse>(
            TRequest request, Func<Task<TResponse>> handler, CancellationToken ct = default)
        {
            var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(typeof(TRequest), typeof(TResponse));
            var behaviors = _serviceProvider.GetServices(behaviorType)
                .Cast<dynamic>()
                .Reverse()
                .ToList();

            Func<Task<TResponse>> pipeline = handler;

            foreach (dynamic behavior in behaviors)
            {
                var next = pipeline;
                pipeline = () => behavior.HandleAsync(request, ct, next);
            }

            return pipeline;
        }
    }
}
