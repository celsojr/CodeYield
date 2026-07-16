namespace CodeYield.Exceptions
{
    /// <summary>
    /// Thrown when a requested entity cannot be found by its identifier.
    /// Carries the entity type name and the identifier that was looked up.
    /// </summary>
    public class NotFoundException : DomainException
    {
        /// <summary>Gets the name of the entity type that was not found.</summary>
        public string? EntityName { get; }

        /// <summary>Gets the identifier that was used to look up the entity.</summary>
        public object? EntityId { get; }

        /// <summary>Initializes a new <see cref="NotFoundException"/> for the specified entity and identifier.</summary>
        public NotFoundException(string entityName, object entityId)
            : base($"{entityName} with identifier {entityId} was not found.")
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}
