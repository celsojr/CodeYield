using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodeYield.EventBus
{
    /// <summary>
    /// Background service that reads events from the channel and delivers them
    /// to configured HTTP subscriber endpoints with automatic retry on failure.
    /// </summary>
    public class EventBusBackgroundService : BackgroundService
    {
        private readonly ChannelReader<EventEnvelope> _reader;
        private readonly EventBusOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EventBusBackgroundService> _logger;

        /// <summary>Initializes a new instance of <see cref="EventBusBackgroundService"/>.</summary>
        public EventBusBackgroundService(
            EventBus eventBus,
            IOptions<EventBusOptions> options,
            IHttpClientFactory httpClientFactory,
            ILogger<EventBusBackgroundService> logger)
        {
            _reader = eventBus.Reader;
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var envelope in _reader.ReadAllAsync(stoppingToken))
            {
                if (!_options.Subscribers.TryGetValue(envelope.EventType, out var subscribers))
                    continue;

                foreach (var subscriber in subscribers)
                {
                    await DeliverWithRetryAsync(envelope, subscriber, stoppingToken);
                }
            }
        }

        private async Task DeliverWithRetryAsync(
            EventEnvelope envelope, SubscriberConfig subscriber, CancellationToken ct)
        {
            var url = ResolveUrl(subscriber.Url, envelope.JsonPayload);

            for (int attempt = 0; attempt <= subscriber.MaxRetries; attempt++)
            {
                try
                {
                    if (_logger.IsEnabled(LogLevel.Debug))
                    {
                        _logger.LogDebug("Delivering {EventType} to {Url} (attempt {Attempt})",
                            envelope.EventType, url, attempt + 1);
                    }

                    using var client = _httpClientFactory.CreateClient();
                    var request = new HttpRequestMessage(new HttpMethod(subscriber.Method), url);

                    if (subscriber.Method.Equals("POST", StringComparison.OrdinalIgnoreCase)
                        && !string.IsNullOrEmpty(envelope.JsonPayload))
                    {
                        request.Content = new StringContent(envelope.JsonPayload, Encoding.UTF8, "application/json");
                    }

                    foreach (var header in subscriber.Headers)
                    {
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }

                    using var response = await client.SendAsync(request, ct);

                    if (response.IsSuccessStatusCode)
                        return;

                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning("Event {EventType} delivery to {Url} returned {Status} (attempt {Attempt})",
                            envelope.EventType, url, response.StatusCode, attempt + 1);
                    }
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning(ex, "Event {EventType} delivery to {Url} failed (attempt {Attempt})",
                            envelope.EventType, url, attempt + 1);
                    }
                }

                if (attempt < subscriber.MaxRetries)
                {
                    var delay = subscriber.RetryDelayMs * (int)Math.Pow(2, attempt);
                    await Task.Delay(delay, ct);
                }
            }

            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError("Event {EventType} delivery to {Url} failed after {MaxRetries} retries",
                    envelope.EventType, url, subscriber.MaxRetries);
            }
        }

        private static string ResolveUrl(string template, string jsonPayload)
        {
            if (!template.Contains('{')) return template;

            using var doc = JsonDocument.Parse(jsonPayload);
            var root = doc.RootElement;

            var result = template;
            foreach (var property in root.EnumerateObject())
            {
                var placeholder = $"{{{property.Name}}}";
                if (result.Contains(placeholder, StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Replace(placeholder, property.Value.ToString(), StringComparison.OrdinalIgnoreCase);
                }
            }
            return result;
        }
    }
}
