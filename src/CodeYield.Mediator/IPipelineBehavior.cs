namespace CodeYield.Mediator
{
    /// <summary>
    /// A pipeline stage that wraps handler execution. Behaviors are chained in registration
    /// order — the first registered runs first, the last registered runs closest to the handler.
    /// Call <paramref name="next"/> to proceed; omit it to short-circuit the pipeline.
    /// </summary>
    /// <typeparam name="TRequest">The command or query type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public interface IPipelineBehavior<in TRequest, TResponse>
    {
        /// <summary>Invokes this behavior. Call <paramref name="next"/> to continue the pipeline.</summary>
        Task<TResponse> HandleAsync(TRequest request, CancellationToken ct, Func<Task<TResponse>> next);
    }
}
