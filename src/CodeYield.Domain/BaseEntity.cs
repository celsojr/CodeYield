using CodeYield.Abstractions;

namespace CodeYield.Domain
{
    /// <summary>
    /// Abstract base class for domain entities that provides identity-based equality.
    /// </summary>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    public abstract class BaseEntity<TId> : IEntity<TId>
    {
        /// <inheritdoc />
        public TId Id { get; protected set; } = default!;

        /// <inheritdoc />
        public override bool Equals(object? obj) =>
            obj is BaseEntity<TId> other && EqualityComparer<TId>.Default.Equals(Id, other.Id);

        /// <inheritdoc />
        public override int GetHashCode() => Id?.GetHashCode() ?? 0;

        /// <summary>Determines whether two entities are equal based on their identifiers.</summary>
        public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right) =>
            left?.Equals(right) ?? right is null;

        /// <summary>Determines whether two entities are not equal based on their identifiers.</summary>
        public static bool operator !=(BaseEntity<TId>? left, BaseEntity<TId>? right) =>
            !(left == right);
    }
}
