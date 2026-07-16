using CodeYield.Domain;

namespace CodeYield.Tests
{
    public class MoneyTests
    {
        [Fact]
        public void Create_ReturnsCorrectValues()
        {
            var money = Money.Create(10.50m, "USD");

            Assert.Equal(10.50m, money.Amount);
            Assert.Equal("USD", money.Currency);
        }

        [Fact]
        public void Create_EmptyCurrency_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Money.Create(10m, ""));
        }

        [Fact]
        public void Zero_ReturnsZeroAmount()
        {
            var zero = Money.Zero("EUR");
            Assert.Equal(0m, zero.Amount);
            Assert.Equal("EUR", zero.Currency);
        }

        [Fact]
        public void Add_SameCurrency_ReturnsSum()
        {
            var a = Money.Create(10m, "USD");
            var b = Money.Create(5m, "USD");

            var result = a.Add(b);

            Assert.Equal(15m, result.Amount);
            Assert.Equal("USD", result.Currency);
        }

        [Fact]
        public void Add_DifferentCurrency_ThrowsInvalidOperationException()
        {
            var a = Money.Create(10m, "USD");
            var b = Money.Create(5m, "EUR");

            Assert.Throws<InvalidOperationException>(() => a.Add(b));
        }

        [Fact]
        public void Subtract_SameCurrency_ReturnsDifference()
        {
            var a = Money.Create(10m, "USD");
            var b = Money.Create(3m, "USD");

            var result = a.Subtract(b);

            Assert.Equal(7m, result.Amount);
        }

        [Fact]
        public void Multiply_ReturnsProduct()
        {
            var money = Money.Create(10m, "USD");
            var result = money.Multiply(2.5m);

            Assert.Equal(25m, result.Amount);
        }

        [Fact]
        public void Equals_SameAmountAndCurrency_ReturnsTrue()
        {
            var a = Money.Create(10m, "USD");
            var b = Money.Create(10m, "USD");

            Assert.Equal(a, b);
        }

        [Fact]
        public void Equals_DifferentAmount_ReturnsFalse()
        {
            var a = Money.Create(10m, "USD");
            var b = Money.Create(20m, "USD");

            Assert.NotEqual(a, b);
        }

        [Fact]
        public void ToString_FormatsCorrectly()
        {
            var money = Money.Create(29.99m, "USD");
            Assert.Equal("29.99 USD", money.ToString());
        }
    }
}
