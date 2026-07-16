using CodeYield.Abstractions;

namespace CodeYield.Domain
{
    /// <summary>
    /// Base class combining aggregate root behavior (domain events) with audit tracking.
    /// Use this when an entity is both an aggregate root and needs creation/update metadata.
    /// </summary>
    /// <typeparam name="TId">The type of the aggregate's identifier.</typeparam>
    public abstract class BaseAggregateRootAuditable<TId> : BaseEntity<TId>, IAggregateRoot, IEditableEntity
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        /// <inheritdoc />
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <inheritdoc />
        public DateTimeOffset CreatedOnUtc { get; protected set; }

        /// <inheritdoc />
        public string? CreatedBy { get; protected set; }

        /// <inheritdoc />
        public DateTimeOffset? UpdatedOnUtc { get; protected set; }

        /// <inheritdoc />
        public string? UpdatedBy { get; protected set; }

        /// <summary>Adds a domain event to be dispatched after the current transaction.</summary>
        protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        /// <inheritdoc />
        public void ClearDomainEvents() => _domainEvents.Clear();

        /// <summary>Sets the audit fields when the entity is first created.</summary>
        protected void SetCreated(DateTimeOffset onUtc, string? by)
        {
            CreatedOnUtc = onUtc;
            CreatedBy = by;
        }

        /// <summary>Sets the audit fields when the entity is updated.</summary>
        protected void SetUpdated(DateTimeOffset onUtc, string? by)
        {
            UpdatedOnUtc = onUtc;
            UpdatedBy = by;
        }
    }
}
