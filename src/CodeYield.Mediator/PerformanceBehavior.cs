using Microsoft.Extensions.Logging;

namespace CodeYield.Mediator
{
    /// <summary>
    /// Logs a warning when a handler exceeds the configured duration threshold.
    /// Default threshold is 500 milliseconds.
    /// </summary>
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
        private readonly long _thresholdMs;

        /// <summary>Initializes a new instance with the default threshold (500ms).</summary>
        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
            : this(logger, 500) { }

        /// <summary>Initializes a new instance with a custom threshold in milliseconds.</summary>
        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger, long thresholdMs)
        {
            _logger = logger;
            _thresholdMs = thresholdMs;
        }

        /// <inheritdoc />
        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken ct, Func<Task<TResponse>> next)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var response = await next();
            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds > _thresholdMs && _logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(
                    "Slow handler detected: {RequestName} took {ElapsedMs}ms (threshold: {ThresholdMs}ms)",
                    typeof(TRequest).Name, stopwatch.ElapsedMilliseconds, _thresholdMs);
            }

            return response;
        }
    }
}
