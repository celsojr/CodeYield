namespace CodeYield.Mediator
{
    /// <summary>
    /// Represents a read-only query that returns a result of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">The type of data returned by the query.</typeparam>
    public interface IQuery<TResponse> { }
}
