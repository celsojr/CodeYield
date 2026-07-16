using CodeYield.Common.Extensions;

namespace CodeYield.Tests
{
    public class StringExtensionTests
    {
        [Fact]
        public void MaskEmail_ShowsFirstCharAndDomain()
        {
            Assert.Equal("j***@example.com", "john@example.com".MaskEmail());
        }

        [Fact]
        public void MaskEmail_SingleCharLocal()
        {
            Assert.Equal("*@example.com", "a@example.com".MaskEmail());
        }

        [Fact]
        public void MaskCreditCard_ShowsFirstAndLastFour()
        {
            Assert.Equal("4111-****-****-1111", "4111111111111111".MaskCreditCard());
        }

        [Fact]
        public void MaskCreditCard_WithDashes()
        {
            Assert.Equal("4111-****-****-1111", "4111-1111-1111-1111".MaskCreditCard());
        }

        [Fact]
        public void MaskPhone_ShowsLastFour()
        {
            Assert.Equal("******5309", "555-867-5309".MaskPhone());
        }

        [Fact]
        public void MaskMiddle_ReplacesMiddleCharacters()
        {
            Assert.Equal("AB***FGH", "ABCDEFGH".MaskMiddle(2, 5));
        }

        [Fact]
        public void MaskMiddle_EntireString()
        {
            Assert.Equal("*****", "ABCDE".MaskMiddle(0, 5));
        }

        [Fact]
        public void MaskMiddle_NothingToMask()
        {
            Assert.Equal("ABC", "ABC".MaskMiddle(2, 2));
        }
    }
}
