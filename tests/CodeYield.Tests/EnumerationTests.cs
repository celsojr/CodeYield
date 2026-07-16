using CodeYield.Domain;

namespace CodeYield.Tests
{
    public class TestEnumeration : Enumeration<TestEnumeration>
    {
        public static readonly TestEnumeration First = new(1, nameof(First));
        public static readonly TestEnumeration Second = new(2, nameof(Second));
        public static readonly TestEnumeration Third = new(3, nameof(Third));

        protected TestEnumeration(int value, string name) : base(value, name) { }
    }

    public class EnumerationTests
    {
        [Fact]
        public void FromValue_ReturnsCorrectInstance()
        {
            var result = TestEnumeration.FromValue(2);
            Assert.Equal(TestEnumeration.Second, result);
        }

        [Fact]
        public void FromValue_InvalidValue_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => TestEnumeration.FromValue(99));
        }

        [Fact]
        public void FromName_ReturnsCorrectInstance()
        {
            var result = TestEnumeration.FromName("Third");
            Assert.Equal(TestEnumeration.Third, result);
        }

        [Fact]
        public void FromName_CaseInsensitive()
        {
            var result = TestEnumeration.FromName("first");
            Assert.Equal(TestEnumeration.First, result);
        }

        [Fact]
        public void FromName_InvalidName_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => TestEnumeration.FromName("Missing"));
        }

        [Fact]
        public void GetAll_ReturnsAllDefinedValues()
        {
            var all = TestEnumeration.GetAll();
            Assert.Equal(3, all.Count);
        }

        [Fact]
        public void Equality_SameValue_ReturnsTrue()
        {
            Assert.Same(TestEnumeration.First, TestEnumeration.First);
        }

        [Fact]
        public void Equality_DifferentValue_ReturnsFalse()
        {
            Assert.True(TestEnumeration.First != TestEnumeration.Second);
        }

        [Fact]
        public void ToString_ReturnsName()
        {
            Assert.Equal("First", TestEnumeration.First.ToString());
        }
    }
}
