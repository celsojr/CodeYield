using System.Text.RegularExpressions;
using CodeYield.Common;

namespace CodeYield.Domain
{
    /// <summary>
    /// Represents a validated email address as a value object.
    /// Use this instead of raw strings to enforce valid email format at the type level.
    /// </summary>
    public sealed class Email : ValueObject<Email>
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>Gets the email address value.</summary>
        public string Value { get; }

        /// <inheritdoc />
        protected override Email EqualityValue => this;

        private Email(string value) => Value = value;

        /// <summary>Creates a new <see cref="Email"/> after validating the format.</summary>
        public static Email Create(string value)
        {
            Guard.AgainstNullOrEmpty(value, nameof(value));
            Guard.AgainstInvalidFormat(value, nameof(value), EmailRegex);
            return new Email(value);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Email other && Value == other.Value;

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();

        /// <inheritdoc />
        public override string ToString() => Value;
    }
}
