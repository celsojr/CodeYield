using Microsoft.Extensions.Logging;

namespace CodeYield.Mediator
{
    /// <summary>
    /// Logs the command or query type, handler name, and execution duration.
    /// Logs at Debug level on entry and Information level on completion.
    /// </summary>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        /// <summary>Initializes a new instance of <see cref="LoggingBehavior{TRequest, TResponse}"/>.</summary>
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken ct, Func<Task<TResponse>> next)
        {
            var requestName = typeof(TRequest).Name;

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling {RequestName}", requestName);
            }

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var response = await next();
            stopwatch.Stop();

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Handled {RequestName} in {ElapsedMs}ms",
                    requestName, stopwatch.ElapsedMilliseconds);
            }

            return response;
        }
    }
}
