namespace CodeYield.Domain
{
    /// <summary>
    /// Abstract base class for value objects that provides structural equality.
    /// Subclasses must override <see cref="GetEqualityComponents"/> to return the values
    /// that define equality for the value object.
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>
        /// Returns the components used to determine structural equality.
        /// Two value objects are equal when all components match in order and value.
        /// </summary>
        protected abstract IEnumerable<object?> GetEqualityComponents();

        /// <inheritdoc />
        public bool Equals(ValueObject? other) =>
            other is not null && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

        /// <inheritdoc />
        public override bool Equals(object? obj) =>
            obj is ValueObject other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() =>
            GetEqualityComponents()
                .Select(x => x?.GetHashCode() ?? 0)
                .Aggregate((a, b) => a ^ b);

        /// <summary>Determines whether two value objects are structurally equal.</summary>
        public static bool operator ==(ValueObject? left, ValueObject? right) =>
            left?.Equals(right) ?? right is null;

        /// <summary>Determines whether two value objects are not structurally equal.</summary>
        public static bool operator !=(ValueObject? left, ValueObject? right) =>
            !(left == right);
    }
}
