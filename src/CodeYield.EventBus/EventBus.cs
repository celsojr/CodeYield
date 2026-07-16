using System.Text.Json;
using System.Threading.Channels;

namespace CodeYield.EventBus
{
    /// <summary>
    /// Channel-based in-process event bus. Events are serialized to JSON and written
    /// to an unbounded channel for asynchronous delivery by <see cref="EventBusBackgroundService"/>.
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly Channel<EventEnvelope> _channel = Channel.CreateUnbounded<EventEnvelope>(
            new UnboundedChannelOptions { SingleReader = true });

        /// <summary>Gets the channel reader used by the background service to consume events.</summary>
        public ChannelReader<EventEnvelope> Reader => _channel.Reader;

        /// <inheritdoc />
        public async Task PublishAsync<T>(T @event, CancellationToken ct = default) where T : class
        {
            var envelope = new EventEnvelope(typeof(T).Name, JsonSerializer.Serialize(@event));
            await _channel.Writer.WriteAsync(envelope, ct);
        }
    }

    /// <summary>
    /// Internal envelope that wraps a serialized event with its type name for routing.
    /// </summary>
    public record EventEnvelope(string EventType, string JsonPayload);
}
