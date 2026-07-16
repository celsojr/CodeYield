namespace CodeYield.Abstractions
{
    /// <summary>
    /// Represents a domain event that has occurred within the system.
    /// Domain events carry metadata for tracing, correlation, and multi-tenancy.
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>Gets the unique identifier of this event instance.</summary>
        Guid EventId { get; }

        /// <summary>Gets the UTC timestamp when the event occurred.</summary>
        DateTimeOffset OccurredOnUtc { get; }

        /// <summary>Gets an optional correlation identifier for distributed tracing.</summary>
        string? CorrelationId { get; }

        /// <summary>Gets the tenant identifier this event belongs to, if applicable.</summary>
        string? TenantId { get; }
    }
}
