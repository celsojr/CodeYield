using System.Runtime.CompilerServices;

namespace CodeYield.Domain
{
    /// <summary>
    /// Base class for type-safe enumerations with behavior. Provides named instances,
    /// value-based lookup, and equality semantics — replacing plain <c>enum</c> types
    /// that cannot carry methods or properties.
    /// </summary>
    /// <example>
    /// <code>
    /// public class OrderStatus : Enumeration&lt;OrderStatus&gt;
    /// {
    ///     public static readonly OrderStatus Pending = new(1, nameof(Pending));
    ///     public static readonly OrderStatus Confirmed = new(2, nameof(Confirmed));
    ///     public static readonly OrderStatus Shipped = new(3, nameof(Shipped));
    ///
    ///     protected OrderStatus(int value, string name) : base(value, name) { }
    ///
    ///     public bool CanTransitionTo(OrderStatus next) =>
    ///         (this == Pending &amp;&amp; next == Confirmed) ||
    ///         (this == Confirmed &amp;&amp; next == Shipped);
    /// }
    /// </code>
    /// </example>
    /// <typeparam name="TEnum">The concrete enumeration type (CRTP).</typeparam>
    public abstract class Enumeration<TEnum> where TEnum : Enumeration<TEnum>
    {
        private static readonly Dictionary<int, TEnum> _instances = new();

        /// <summary>Gets the integer value of the enumeration member.</summary>
        public int Value { get; }

        /// <summary>Gets the name of the enumeration member.</summary>
        public string Name { get; }

        /// <summary>Initializes a new enumeration member and registers it for lookup.</summary>
        protected Enumeration(int value, string name)
        {
            Value = value;
            Name = name;
            _instances[value] = (TEnum)this;
        }

        /// <summary>Ensures the derived enumeration type's static members are initialized.</summary>
        private static void EnsureInitialized() =>
            RuntimeHelpers.RunClassConstructor(typeof(TEnum).TypeHandle);

        /// <summary>Looks up an enumeration member by its integer value.</summary>
        public static TEnum FromValue(int value)
        {
            EnsureInitialized();
            return _instances.TryGetValue(value, out var instance)
                ? instance
                : throw new InvalidOperationException($"Value '{value}' is not a valid {typeof(TEnum).Name}.");
        }

        /// <summary>Looks up an enumeration member by its name (case-insensitive).</summary>
        public static TEnum FromName(string name)
        {
            EnsureInitialized();
            return _instances.Values.FirstOrDefault(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"Name '{name}' is not a valid {typeof(TEnum).Name}.");
        }

        /// <summary>Returns all defined enumeration members.</summary>
        public static IReadOnlyCollection<TEnum> GetAll()
        {
            EnsureInitialized();
            return _instances.Values.ToList().AsReadOnly();
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) =>
            obj is Enumeration<TEnum> other && Value == other.Value;

        /// <inheritdoc />
        public override int GetHashCode() => Value;

        /// <inheritdoc />
        public override string ToString() => Name;

        /// <summary>Determines whether two enumeration members are equal by value.</summary>
        public static bool operator ==(Enumeration<TEnum>? left, Enumeration<TEnum>? right) =>
            left?.Equals(right) ?? right is null;

        /// <summary>Determines whether two enumeration members are not equal by value.</summary>
        public static bool operator !=(Enumeration<TEnum>? left, Enumeration<TEnum>? right) =>
            !(left == right);
    }
}
