namespace CodeYield.Abstractions
{
    /// <summary>
    /// Represents a domain entity with a strongly-typed identifier.
    /// </summary>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    public interface IEntity<out TId>
    {
        /// <summary>Gets the unique identifier of the entity.</summary>
        TId Id { get; }
    }
}
