using CodeYield.Mediator;

namespace CodeYield.Tests
{
    public class ResultTests
    {
        [Fact]
        public void Success_ReturnsSuccessResult()
        {
            var result = Result.Success();
            Assert.True(result.IsSuccess);
            Assert.Null(result.Error);
        }

        [Fact]
        public void Failure_ReturnsFailedResult()
        {
            var result = Result.Failure("something went wrong");
            Assert.False(result.IsSuccess);
            Assert.Equal("something went wrong", result.Error);
        }

        [Fact]
        public void Success_WithData_ReturnsData()
        {
            var result = Result<int>.Success(42);
            Assert.True(result.IsSuccess);
            Assert.Equal(42, result.Data);
        }

        [Fact]
        public void Success_WithData_HasNullError()
        {
            var result = Result<string>.Success("hello");
            Assert.Null(result.Error);
        }

        [Fact]
        public void Failure_WithData_ReturnsDefaultData()
        {
            var result = Result<int>.Failure("error");
            Assert.False(result.IsSuccess);
            Assert.Equal(0, result.Data);
            Assert.Equal("error", result.Error);
        }

        [Fact]
        public void Result_InheritsFromResult()
        {
            var result = Result<int>.Success(5);
            Assert.IsAssignableFrom<Result>(result);
        }
    }
}
