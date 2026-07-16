using CodeYield.Common;

namespace CodeYield.Tests
{
    public class AsyncLazyTests
    {
        [Fact]
        public async Task Value_ExecutesFactoryOnce()
        {
            int callCount = 0;
            var lazy = new AsyncLazy<int>(async () =>
            {
                Interlocked.Increment(ref callCount);
                await Task.Delay(10);
                return 42;
            });

            var a = await lazy.Value;
            var b = await lazy.Value;

            Assert.Equal(42, a);
            Assert.Equal(42, b);
            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task IsValueCreated_BeforeAccess_ReturnsFalse()
        {
            var lazy = new AsyncLazy<int>(async () => await Task.FromResult(1));
            Assert.False(lazy.IsValueCreated);
        }

        [Fact]
        public async Task IsValueCreated_AfterAccess_ReturnsTrue()
        {
            var lazy = new AsyncLazy<int>(async () => await Task.FromResult(1));
            await lazy.Value;
            Assert.True(lazy.IsValueCreated);
        }
    }
}
