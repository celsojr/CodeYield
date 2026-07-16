using CodeYield.Abstractions;

namespace CodeYield.Mediator
{
    /// <summary>
    /// Handles a domain event that has been raised by an aggregate root.
    /// Implementations perform side effects (e.g., sending notifications, updating read models)
    /// in response to state changes.
    /// </summary>
    /// <typeparam name="TEvent">The domain event type.</typeparam>
    public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
    {
        /// <summary>Handles the domain event.</summary>
        Task HandleAsync(TEvent domainEvent, CancellationToken ct = default);
    }
}
