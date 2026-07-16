using CodeYield.Common;

namespace CodeYield.Tests
{
    public class RetryPolicyTests
    {
        [Fact]
        public async Task ExecuteAsync_SucceedsFirstAttempt()
        {
            var result = await RetryPolicy.ExecuteAsync(
                async () => await Task.FromResult(42),
                maxRetries: 3);

            Assert.Equal(42, result);
        }

        [Fact]
        public async Task ExecuteAsync_SucceedsAfterRetries()
        {
            int attempts = 0;
            var result = await RetryPolicy.ExecuteAsync(
                async () =>
                {
                    Interlocked.Increment(ref attempts);
                    if (attempts < 3)
                        throw new InvalidOperationException("fail");
                    return "ok";
                },
                maxRetries: 3,
                baseDelay: TimeSpan.FromMilliseconds(1));

            Assert.Equal("ok", result);
            Assert.Equal(3, attempts);
        }

        [Fact]
        public async Task ExecuteAsync_ExhaustsRetries_ThrowsLastException()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await RetryPolicy.ExecuteAsync<string>(
                    async () =>
                    {
                        throw new InvalidOperationException("always fail");
                    },
                    maxRetries: 2,
                    baseDelay: TimeSpan.FromMilliseconds(1));
            });
        }

        [Fact]
        public async Task ExecuteAsync_RetryOn_FiltersExceptions()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await RetryPolicy.ExecuteAsync<string>(
                    async () =>
                    {
                        throw new ArgumentException("not retried");
                    },
                    maxRetries: 3,
                    baseDelay: TimeSpan.FromMilliseconds(1),
                    retryOn: ex => ex is InvalidOperationException);
            });
        }

        [Fact]
        public async Task ExecuteAsync_VoidOverload_Works()
        {
            int count = 0;
            await RetryPolicy.ExecuteAsync(
                async () => { count++; await Task.CompletedTask; },
                maxRetries: 1,
                baseDelay: TimeSpan.FromMilliseconds(1));

            Assert.Equal(1, count);
        }
    }
}
