using CodeYield.Common.Clock;

namespace CodeYield.Tests
{
    public class ClockTests
    {
        [Fact]
        public void SystemClock_ReturnsCurrentTime()
        {
            var before = DateTimeOffset.UtcNow.AddSeconds(-1);
            var clock = SystemClock.Instance;
            var after = DateTimeOffset.UtcNow.AddSeconds(1);

            Assert.InRange(clock.UtcNow, before, after);
        }
    }
}
