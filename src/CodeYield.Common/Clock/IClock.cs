namespace CodeYield.Common.Clock
{
    /// <summary>
    /// Abstracts the current time for testability. Inject this instead of calling
    /// <c>DateTimeOffset.UtcNow</c> directly so tests can freeze or advance time.
    /// </summary>
    public interface IClock
    {
        /// <summary>Gets the current UTC date and time.</summary>
        DateTimeOffset UtcNow { get; }
    }
}
