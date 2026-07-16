namespace CodeYield.EventBus
{
    /// <summary>
    /// Configuration for the event bus, mapping event type names to their subscriber endpoints.
    /// </summary>
    public class EventBusOptions
    {
        /// <summary>
        /// Gets or sets the subscriber configurations keyed by event type name.
        /// Each event type can have multiple subscribers.
        /// </summary>
        public Dictionary<string, List<SubscriberConfig>> Subscribers { get; set; } = [];
    }

    /// <summary>
    /// Describes a single HTTP endpoint that subscribes to a specific event type,
    /// including retry behavior.
    /// </summary>
    public class SubscriberConfig
    {
        /// <summary>The URL to deliver events to. Supports <c>{PropertyName}</c> placeholders
        /// resolved from the event JSON payload.</summary>
        public required string Url { get; set; }

        /// <summary>The HTTP method to use (default: POST).</summary>
        public string Method { get; set; } = "POST";

        /// <summary>Optional HTTP headers to include with each request.</summary>
        public Dictionary<string, string> Headers { get; set; } = [];

        /// <summary>Maximum number of retry attempts on failure (default: 3).</summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>Base delay in milliseconds between retries. Doubles on each attempt (default: 500).</summary>
        public int RetryDelayMs { get; set; } = 500;
    }
}
