using System.ComponentModel;
using CodeYield.Common.Extensions;

namespace CodeYield.Tests
{
    public class EnumExtensionTests
    {
        private enum TestPriority
        {
            [Description("Low priority")]
            Low,

            [Description("High priority")]
            High,

            NoDescription
        }

        [Fact]
        public void GetDescription_WithAttribute_ReturnsDescription()
        {
            Assert.Equal("Low priority", TestPriority.Low.GetDescription());
        }

        [Fact]
        public void GetDescription_WithoutAttribute_ReturnsName()
        {
            Assert.Equal("NoDescription", TestPriority.NoDescription.GetDescription());
        }

        [Fact]
        public void GetValues_ReturnsAllDefinedValues()
        {
            var values = EnumExtensions.GetValues<TestPriority>();
            Assert.Equal(3, values.Length);
        }
    }
}
