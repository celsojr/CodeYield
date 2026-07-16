using CodeYield.Domain;

namespace CodeYield.Tests
{
    public class EmailTests
    {
        [Theory]
        [InlineData("user@example.com")]
        [InlineData("test.user@domain.co.uk")]
        [InlineData("a+b@c.com")]
        public void Create_ValidEmail_ReturnsEmail(string input)
        {
            var email = Email.Create(input);
            Assert.Equal(input, email.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_NullOrEmpty_ThrowsArgumentException(string? input)
        {
            Assert.Throws<ArgumentException>(() => Email.Create(input!));
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("@domain.com")]
        [InlineData("user@")]
        [InlineData("user@.com")]
        public void Create_InvalidFormat_ThrowsArgumentException(string input)
        {
            Assert.Throws<ArgumentException>(() => Email.Create(input));
        }

        [Fact]
        public void Equals_SameValue_ReturnsTrue()
        {
            var a = Email.Create("user@example.com");
            var b = Email.Create("user@example.com");

            Assert.Equal(a, b);
            Assert.True(a == b);
        }

        [Fact]
        public void Equals_DifferentValue_ReturnsFalse()
        {
            var a = Email.Create("user@example.com");
            var b = Email.Create("other@example.com");

            Assert.NotEqual(a, b);
            Assert.True(a != b);
        }

        [Fact]
        public void ToString_ReturnsValue()
        {
            var email = Email.Create("user@example.com");
            Assert.Equal("user@example.com", email.ToString());
        }
    }
}
