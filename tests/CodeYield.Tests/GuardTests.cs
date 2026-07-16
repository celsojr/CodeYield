using CodeYield.Common;

namespace CodeYield.Tests
{
    public class GuardTests
    {
        [Fact]
        public void AgainstNull_WithNull_ThrowsArgumentNullException()
        {
            var ex = Record.Exception(() => Guard.AgainstNull(null!, "value"));
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void AgainstNull_WithNonNull_DoesNotThrow()
        {
            Guard.AgainstNull("hello", "value");
        }

        [Fact]
        public void AgainstNullOrEmpty_WithNull_ThrowsArgumentException()
        {
            var ex = Record.Exception(() => Guard.AgainstNullOrEmpty(null, "value"));
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void AgainstNullOrEmpty_WithEmpty_ThrowsArgumentException()
        {
            var ex = Record.Exception(() => Guard.AgainstNullOrEmpty("", "value"));
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void AgainstNullOrEmpty_WithValue_DoesNotThrow()
        {
            Guard.AgainstNullOrEmpty("hello", "value");
        }

        [Fact]
        public void AgainstNullOrWhiteSpace_WithWhitespace_ThrowsArgumentException()
        {
            var ex = Record.Exception(() => Guard.AgainstNullOrWhiteSpace("   ", "value"));
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void AgainstNegative_WithNegative_ThrowsArgumentOutOfRangeException()
        {
            var ex = Record.Exception(() => Guard.AgainstNegative(-1, "value"));
            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }

        [Fact]
        public void AgainstNegative_WithZero_DoesNotThrow()
        {
            Guard.AgainstNegative(0, "value");
        }

        [Fact]
        public void AgainstNegativeOrZero_WithZero_ThrowsArgumentOutOfRangeException()
        {
            var ex = Record.Exception(() => Guard.AgainstNegativeOrZero(0, "value"));
            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }

        [Fact]
        public void AgainstNegativeOrZero_WithPositive_DoesNotThrow()
        {
            Guard.AgainstNegativeOrZero(1, "value");
        }

        [Fact]
        public void AgainstInvalidFormat_WithNonMatching_ThrowsArgumentException()
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^\d+$");
            var ex = Record.Exception(() => Guard.AgainstInvalidFormat("abc", "value", regex));
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void AgainstInvalidFormat_WithMatching_DoesNotThrow()
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^\d+$");
            Guard.AgainstInvalidFormat("123", "value", regex);
        }
    }
}
