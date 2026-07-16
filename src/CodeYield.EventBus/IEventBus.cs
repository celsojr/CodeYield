namespace CodeYield.EventBus
{
    /// <summary>
    /// Contract for an in-process event bus that publishes domain events
    /// to subscribers via a channel-based pipeline.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publishes a domain event to all registered subscribers.
        /// </summary>
        /// <typeparam name="T">The type of the event.</typeparam>
        /// <param name="event">The event instance to publish.</param>
        /// <param name="ct">Cancellation token.</param>
        Task PublishAsync<T>(T @event, CancellationToken ct = default) where T : class;
    }
}
