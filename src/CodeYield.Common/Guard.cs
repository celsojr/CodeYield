using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CodeYield.Common
{
    /// <summary>
    /// Guard clauses for parameter validation. Throws immediately when a precondition fails,
    /// eliminating repetitive null-check boilerplate.
    /// </summary>
    public static class Guard
    {
        /// <summary>Throws <see cref="ArgumentNullException"/> if <paramref name="value"/> is null.</summary>
        public static void AgainstNull([NotNull] object? value, string name)
        {
            if (value is null) throw new ArgumentNullException(name);
        }

        /// <summary>Throws <see cref="ArgumentNullException"/> or <see cref="ArgumentException"/> if <paramref name="value"/> is null or empty.</summary>
        public static void AgainstNullOrEmpty([NotNull] string? value, string name)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{name} cannot be null or empty.", name);
        }

        /// <summary>Throws <see cref="ArgumentException"/> if <paramref name="value"/> is null, empty, or consists only of whitespace.</summary>
        public static void AgainstNullOrWhiteSpace([NotNull] string? value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{name} cannot be null, empty, or whitespace.", name);
        }

        /// <summary>Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.</summary>
        public static void AgainstNegative(double value, string name)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(name, value, $"{name} must not be negative.");
        }

        /// <summary>Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative or zero.</summary>
        public static void AgainstNegativeOrZero(double value, string name)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(name, value, $"{name} must be greater than zero.");
        }

        /// <summary>Throws <see cref="ArgumentException"/> if <paramref name="value"/> does not match <paramref name="pattern"/>.</summary>
        public static void AgainstInvalidFormat(string value, string name, Regex pattern)
        {
            if (!pattern.IsMatch(value))
                throw new ArgumentException($"{name} does not match the required format.", name);
        }

        /// <summary>Throws <see cref="ArgumentException"/> if <paramref name="value"/> is not one of the specified <paramref name="validValues"/>.</summary>
        public static void AgainstInvalidEnumValue<T>(T value, string name) where T : struct, Enum
        {
            if (!Enum.IsDefined(value))
                throw new ArgumentOutOfRangeException(name, value, $"{value} is not a valid {typeof(T).Name}.");
        }
    }
}
