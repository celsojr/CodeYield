using CodeYield.Common;

namespace CodeYield.Domain
{
    /// <summary>
    /// Represents a monetary amount with a currency code as a value object.
    /// Enforces currency consistency when combining amounts.
    /// </summary>
    public sealed class Money : ValueObject
    {
        /// <summary>Gets the monetary amount.</summary>
        public decimal Amount { get; }

        /// <summary>Gets the ISO 4217 currency code (e.g., "USD", "EUR").</summary>
        public string Currency { get; }

        /// <inheritdoc />
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        /// <summary>Creates a new <see cref="Money"/> instance.</summary>
        public static Money Create(decimal amount, string currency)
        {
            Guard.AgainstNullOrEmpty(currency, nameof(currency));
            return new Money(amount, currency);
        }

        /// <summary>Creates a zero-amount <see cref="Money"/> with the specified currency.</summary>
        public static Money Zero(string currency) => new(0, currency);

        /// <summary>Adds another <see cref="Money"/> with the same currency.</summary>
        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException(
                    $"Cannot add money with different currencies: {Currency} and {other.Currency}.");
            return new Money(Amount + other.Amount, Currency);
        }

        /// <summary>Subtracts another <see cref="Money"/> with the same currency.</summary>
        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException(
                    $"Cannot subtract money with different currencies: {Currency} and {other.Currency}.");
            return new Money(Amount - other.Amount, Currency);
        }

        /// <summary>Multiplies the amount by a factor.</summary>
        public Money Multiply(decimal factor) => new(Amount * factor, Currency);

        /// <inheritdoc />
        public override string ToString() => $"{Amount:N2} {Currency}";
    }
}
