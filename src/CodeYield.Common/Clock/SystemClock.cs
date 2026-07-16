namespace CodeYield.Common.Clock
{
    /// <summary>
    /// Default clock implementation that returns the real system time.
    /// Register as <see cref="IClock"/> in the DI container for production use.
    /// </summary>
    public sealed class SystemClock : IClock
    {
        /// <summary>A shared singleton instance.</summary>
        public static readonly SystemClock Instance = new();

        /// <inheritdoc />
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
