using CodeYield.Abstractions;

namespace CodeYield.Domain
{
    /// <summary>
    /// Base class for aggregate roots that collects domain events and provides identity-based equality.
    /// </summary>
    /// <typeparam name="TId">The type of the aggregate's identifier.</typeparam>
    public abstract class BaseAggregateRoot<TId> : BaseEntity<TId>, IAggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        /// <inheritdoc />
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>Adds a domain event to be dispatched after the current transaction.</summary>
        protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        /// <inheritdoc />
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
