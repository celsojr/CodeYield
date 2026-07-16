using CodeYield.Common.Collections;
using CodeYield.Common.Extensions;

namespace CodeYield.Tests
{
    public class CyListTests
    {
        [Fact]
        public void ToString_DisplaysBracketDelimited()
        {
            var list = new CyList<int> { 1, 2, 3 };
            Assert.Equal("[1, 2, 3]", list.ToString());
        }

        [Fact]
        public void ToString_EmptyList()
        {
            var list = new CyList<string>();
            Assert.Equal("[]", list.ToString());
        }

        [Fact]
        public void ToString_SingleItem()
        {
            var list = new CyList<string> { "hello" };
            Assert.Equal("[hello]", list.ToString());
        }

        [Fact]
        public void GetLoopContext_ReturnsCorrectCount()
        {
            var list = new CyList<int> { 1, 2, 3, 4, 5 };
            var contexts = list.GetLoopContext().ToList();

            Assert.Equal(5, contexts.Count);
        }

        [Fact]
        public void GetLoopContext_FirstIsFirst()
        {
            var list = new CyList<int> { 1, 2, 3 };
            var first = list.GetLoopContext().First();

            Assert.True(first.IsFirst);
            Assert.Equal(0, first.Index);
        }

        [Fact]
        public void GetLoopContext_LastIsLast()
        {
            var list = new CyList<int> { 1, 2, 3 };
            var last = list.GetLoopContext().Last();

            Assert.True(last.IsLast);
            Assert.Equal(2, last.Index);
        }

        [Fact]
        public void GetLoopContext_NextAndPrev()
        {
            var list = new CyList<int> { 10, 20, 30 };
            var contexts = list.GetLoopContext().ToList();

            Assert.Equal(20, contexts[0].Next);
            Assert.Equal(10, contexts[1].Prev);
        }

        [Fact]
        public void AsLoop_ReturnsBracketDelimited()
        {
            var numbers = new List<int> { 1, 2, 3 };
            var result = numbers.AsLoop().ToString();
            Assert.Equal("[1, 2, 3]", result);
        }
    }
}
