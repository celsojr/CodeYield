namespace CodeYield.Abstractions
{
    /// <summary>
    /// Marks a domain entity as an aggregate root, the consistency boundary within which
    /// domain invariants are enforced. Aggregate roots collect domain events that are
    /// dispatched after the transaction commits.
    /// </summary>
    public interface IAggregateRoot
    {
        /// <summary>Gets the collection of domain events raised by this aggregate.</summary>
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        /// <summary>Clears all pending domain events after they have been dispatched.</summary>
        void ClearDomainEvents();
    }
}
