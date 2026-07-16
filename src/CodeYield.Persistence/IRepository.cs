using CodeYield.Abstractions;

namespace CodeYield.Persistence
{
    /// <summary>
    /// Generic repository interface providing standard CRUD operations and paginated queries
    /// for domain entities.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    public interface IRepository<T, in TId> where T : IEntity<TId>
    {
        /// <summary>Retrieves an entity by its identifier, or null if not found.</summary>
        Task<T?> GetByIdAsync(TId id, CancellationToken ct = default);

        /// <summary>Retrieves a paginated list of all entities.</summary>
        Task<PaginatedResult<T>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);

        /// <summary>Persists a new entity and returns it.</summary>
        Task<T> AddAsync(T entity, CancellationToken ct = default);

        /// <summary>Updates an existing entity.</summary>
        Task UpdateAsync(T entity, CancellationToken ct = default);

        /// <summary>Deletes an entity.</summary>
        Task DeleteAsync(T entity, CancellationToken ct = default);
    }
}
