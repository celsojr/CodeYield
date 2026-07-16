using CodeYield.Domain;

namespace CodeYield.Tests
{
    public class BaseEntityTests
    {
        private class TestEntity : BaseEntity<Guid>
        {
            public TestEntity(Guid id) => Id = id;
        }

        [Fact]
        public void Equals_SameId_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            var a = new TestEntity(id);
            var b = new TestEntity(id);

            Assert.Equal(a, b);
        }

        [Fact]
        public void Equals_DifferentId_ReturnsFalse()
        {
            var a = new TestEntity(Guid.NewGuid());
            var b = new TestEntity(Guid.NewGuid());

            Assert.NotEqual(a, b);
        }

        [Fact]
        public void OperatorEquals_SameId_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            var a = new TestEntity(id);
            var b = new TestEntity(id);

            Assert.True(a == b);
        }

        [Fact]
        public void OperatorNotEquals_DifferentId_ReturnsTrue()
        {
            var a = new TestEntity(Guid.NewGuid());
            var b = new TestEntity(Guid.NewGuid());

            Assert.True(a != b);
        }

        [Fact]
        public void GetHashCode_SameId_SameHash()
        {
            var id = Guid.NewGuid();
            var a = new TestEntity(id);
            var b = new TestEntity(id);

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void Equals_Null_ReturnsFalse()
        {
            var a = new TestEntity(Guid.NewGuid());
            Assert.False(a.Equals(null));
        }
    }
}
