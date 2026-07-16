using CodeYield.Abstractions;

namespace CodeYield.Mediator
{
    /// <summary>
    /// Central dispatcher for commands, queries, and domain events.
    /// Resolves handlers from the DI container and invokes them.
    /// This is a lightweight interface designed to work alongside CodeYield's CQRS contracts.
    /// If you already use MediatR, see the README for a coexistence adapter example.
    /// </summary>
    public interface IMediator
    {
        /// <summary>Dispatches a command that does not return a value.</summary>
        Task SendAsync(ICommand command, CancellationToken ct = default);

        /// <summary>Dispatches a command and returns the response.</summary>
        Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct = default);

        /// <summary>Dispatches a query and returns the result.</summary>
        Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct = default);

        /// <summary>Publishes a domain event to all registered handlers.</summary>
        Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken ct = default) where TEvent : IDomainEvent;
    }
}
