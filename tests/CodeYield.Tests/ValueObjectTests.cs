using CodeYield.Domain;

namespace CodeYield.Tests
{
    public class ValueObjectTests
    {
        private class Address : ValueObject
        {
            public string Street { get; }
            public string City { get; }

            public Address(string street, string city)
            {
                Street = street;
                City = city;
            }

            protected override IEnumerable<object?> GetEqualityComponents()
            {
                yield return Street;
                yield return City;
            }
        }

        [Fact]
        public void Equals_SameComponents_ReturnsTrue()
        {
            var a = new Address("123 Main St", "Springfield");
            var b = new Address("123 Main St", "Springfield");

            Assert.Equal(a, b);
        }

        [Fact]
        public void Equals_DifferentComponents_ReturnsFalse()
        {
            var a = new Address("123 Main St", "Springfield");
            var b = new Address("456 Oak Ave", "Shelbyville");

            Assert.NotEqual(a, b);
        }

        [Fact]
        public void OperatorEquals_ReturnsTrue()
        {
            var a = new Address("123 Main St", "Springfield");
            var b = new Address("123 Main St", "Springfield");

            Assert.True(a == b);
        }

        [Fact]
        public void OperatorNotEquals_ReturnsTrue()
        {
            var a = new Address("123 Main St", "Springfield");
            var b = new Address("456 Oak Ave", "Shelbyville");

            Assert.True(a != b);
        }

        [Fact]
        public void GetHashCode_SameComponents_SameHash()
        {
            var a = new Address("123 Main St", "Springfield");
            var b = new Address("123 Main St", "Springfield");

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }
    }

    public class ValueObjectGenericTests
    {
        private class SingleValue : ValueObject<SingleValue>
        {
            public string Value { get; }
            protected override SingleValue EqualityValue => this;

            public SingleValue(string value) => Value = value;

            public override bool Equals(object? obj) =>
                obj is SingleValue other && Value == other.Value;

            public override int GetHashCode() => Value.GetHashCode();
        }

        [Fact]
        public void Equals_SameValue_ReturnsTrue()
        {
            var a = new SingleValue("hello");
            var b = new SingleValue("hello");

            Assert.Equal(a, b);
        }

        [Fact]
        public void Equals_DifferentValue_ReturnsFalse()
        {
            var a = new SingleValue("hello");
            var b = new SingleValue("world");

            Assert.NotEqual(a, b);
        }
    }
}
