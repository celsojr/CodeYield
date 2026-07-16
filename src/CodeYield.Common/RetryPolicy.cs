namespace CodeYield.Common
{
    /// <summary>
    /// Simple retry executor with configurable attempts and exponential backoff.
    /// </summary>
    public static class RetryPolicy
    {
        /// <summary>
        /// Executes the action with retry. Retries up to <paramref name="maxRetries"/> times
        /// with exponential backoff starting at <paramref name="baseDelay"/>.
        /// </summary>
        public static async Task<T> ExecuteAsync<T>(
            Func<Task<T>> action,
            int maxRetries = 3,
            TimeSpan? baseDelay = null,
            Func<Exception, bool>? retryOn = null,
            CancellationToken ct = default)
        {
            var delay = baseDelay ?? TimeSpan.FromMilliseconds(500);

            for (int attempt = 0; ; attempt++)
            {
                try
                {
                    ct.ThrowIfCancellationRequested();
                    return await action();
                }
                catch (Exception ex) when (attempt < maxRetries && (retryOn is null || retryOn(ex)))
                {
                    var wait = delay * (int)Math.Pow(2, attempt);
                    await Task.Delay(wait, ct);
                }
            }
        }

        /// <summary>
        /// Executes the action with retry. Retries up to <paramref name="maxRetries"/> times
        /// with exponential backoff starting at <paramref name="baseDelay"/>.
        /// </summary>
        public static async Task ExecuteAsync(
            Func<Task> action,
            int maxRetries = 3,
            TimeSpan? baseDelay = null,
            Func<Exception, bool>? retryOn = null,
            CancellationToken ct = default)
        {
            await ExecuteAsync<object>(
                async () =>
                {
                    await action();
                    return null!;
                },
                maxRetries, baseDelay, retryOn, ct);
        }
    }
}
