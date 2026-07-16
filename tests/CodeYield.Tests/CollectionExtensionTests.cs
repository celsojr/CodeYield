using CodeYield.Common.Extensions;

namespace CodeYield.Tests
{
    public class CollectionExtensionTests
    {
        [Fact]
        public void AddIf_ConditionTrue_Adds()
        {
            var list = new List<int>();
            list.AddIf(true, 42);
            Assert.Single(list);
            Assert.Equal(42, list[0]);
        }

        [Fact]
        public void AddIf_ConditionFalse_DoesNotAdd()
        {
            var list = new List<int>();
            list.AddIf(false, 42);
            Assert.Empty(list);
        }

        [Fact]
        public void AddIfNotNull_WithNull_DoesNotAdd()
        {
            var list = new List<string>();
            list.AddIfNotNull(null);
            Assert.Empty(list);
        }

        [Fact]
        public void AddIfNotNull_WithValue_Adds()
        {
            var list = new List<string>();
            list.AddIfNotNull("hello");
            Assert.Single(list);
            Assert.Equal("hello", list[0]);
        }

        [Fact]
        public void AddRange_AddsAllElements()
        {
            var list = new List<int> { 1, 2 };
            list.AddRange(new[] { 3, 4, 5 });
            Assert.Equal(5, list.Count);
        }

        [Fact]
        public void RemoveAll_RemovesMatchingElements()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };
            var removed = list.RemoveAll(x => x % 2 == 0);
            Assert.Equal(2, removed);
            Assert.Equal(new[] { 1, 3, 5 }, list);
        }

        [Fact]
        public void ForEach_ExecutesAction()
        {
            var list = new List<int> { 1, 2, 3 };
            var sum = 0;
            list.ForEach(x => sum += x);
            Assert.Equal(6, sum);
        }

        [Fact]
        public void Shuffle_ChangesOrder()
        {
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var original = new List<int>(list);

            list.Shuffle();

            Assert.Equal(original.Count, list.Count);
            Assert.Equal(original.OrderBy(x => x), list.OrderBy(x => x));
        }

        [Fact]
        public void IndexOf_FindsMatchingElement()
        {
            var list = new List<int> { 10, 20, 30, 40 };
            var index = list.IndexOf(x => x == 30);
            Assert.Equal(2, index);
        }

        [Fact]
        public void IndexOf_NotFound_ReturnsNegativeOne()
        {
            var list = new List<int> { 10, 20, 30 };
            var index = list.IndexOf(x => x == 99);
            Assert.Equal(-1, index);
        }
    }
}
