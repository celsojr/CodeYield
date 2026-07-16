namespace CodeYield.Domain
{
    /// <summary>
    /// Generic value object with a single equality component. For value objects with
    /// multiple components, derive from <see cref="ValueObject"/> instead.
    /// </summary>
    /// <typeparam name="T">The type of the equality component.</typeparam>
    public abstract class ValueObject<T> : IEquatable<ValueObject<T>>
    {
        /// <summary>
        /// Returns the single value used to determine structural equality.
        /// </summary>
        protected abstract T EqualityValue { get; }

        /// <inheritdoc />
        public bool Equals(ValueObject<T>? other) =>
            other is not null && EqualityComparer<T>.Default.Equals(EqualityValue, other.EqualityValue);

        /// <inheritdoc />
        public override bool Equals(object? obj) =>
            obj is ValueObject<T> other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() =>
            EqualityValue?.GetHashCode() ?? 0;

        /// <summary>Determines whether two value objects are structurally equal.</summary>
        public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right) =>
            left?.Equals(right) ?? right is null;

        /// <summary>Determines whether two value objects are not structurally equal.</summary>
        public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right) =>
            !(left == right);
    }
}
