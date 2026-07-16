namespace CodeYield.Mediator
{
    /// <summary>
    /// Handles a read-only query and returns a result.
    /// </summary>
    /// <typeparam name="TQuery">The query type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        /// <summary>Executes the query and returns the result.</summary>
        Task<TResponse> HandleAsync(TQuery query, CancellationToken ct = default);
    }
}
