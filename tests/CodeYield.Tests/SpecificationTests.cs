using System.Linq.Expressions;
using CodeYield.Persistence;

namespace CodeYield.Tests
{
    public class SpecificationTests
    {
        private record Product(string Name, decimal Price, bool IsActive);

        private class ActiveProducts : Specification<Product>
        {
            public override Expression<Func<Product, bool>> ToExpression() =>
                p => p.IsActive;
        }

        private class CheapProducts : Specification<Product>
        {
            public override Expression<Func<Product, bool>> ToExpression() =>
                p => p.Price < 10m;
        }

        [Fact]
        public void IsSatisfiedBy_MatchingEntity_ReturnsTrue()
        {
            var spec = new ActiveProducts();
            var product = new Product("Widget", 5m, IsActive: true);

            Assert.True(spec.IsSatisfiedBy(product));
        }

        [Fact]
        public void IsSatisfiedBy_NonMatchingEntity_ReturnsFalse()
        {
            var spec = new ActiveProducts();
            var product = new Product("Widget", 5m, IsActive: false);

            Assert.False(spec.IsSatisfiedBy(product));
        }

        [Fact]
        public void And_BothMatch_ReturnsTrue()
        {
            var spec = new ActiveProducts().And(new CheapProducts());
            var product = new Product("Widget", 5m, IsActive: true);

            Assert.True(spec.IsSatisfiedBy(product));
        }

        [Fact]
        public void And_OnlyOneMatches_ReturnsFalse()
        {
            var spec = new ActiveProducts().And(new CheapProducts());
            var product = new Product("Widget", 50m, IsActive: true);

            Assert.False(spec.IsSatisfiedBy(product));
        }

        [Fact]
        public void Or_OneMatches_ReturnsTrue()
        {
            var spec = new ActiveProducts().Or(new CheapProducts());
            var product = new Product("Widget", 5m, IsActive: false);

            Assert.True(spec.IsSatisfiedBy(product));
        }

        [Fact]
        public void Or_NoneMatch_ReturnsFalse()
        {
            var spec = new ActiveProducts().Or(new CheapProducts());
            var product = new Product("Widget", 50m, IsActive: false);

            Assert.False(spec.IsSatisfiedBy(product));
        }

        [Fact]
        public void Not_MatchingEntity_ReturnsFalse()
        {
            var spec = new ActiveProducts().Not();
            var product = new Product("Widget", 5m, IsActive: true);

            Assert.False(spec.IsSatisfiedBy(product));
        }

        [Fact]
        public void Not_NonMatchingEntity_ReturnsTrue()
        {
            var spec = new ActiveProducts().Not();
            var product = new Product("Widget", 5m, IsActive: false);

            Assert.True(spec.IsSatisfiedBy(product));
        }
    }
}
