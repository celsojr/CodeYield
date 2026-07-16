namespace CodeYield.Abstractions
{
    /// <summary>
    /// Provides audit tracking properties for entities that support creation and update metadata.
    /// </summary>
    public interface IEditableEntity
    {
        /// <summary>Gets the UTC timestamp when the entity was created.</summary>
        DateTimeOffset CreatedOnUtc { get; }

        /// <summary>Gets the identifier of the user who created the entity.</summary>
        string? CreatedBy { get; }

        /// <summary>Gets the UTC timestamp when the entity was last updated, if applicable.</summary>
        DateTimeOffset? UpdatedOnUtc { get; }

        /// <summary>Gets the identifier of the user who last updated the entity, if applicable.</summary>
        string? UpdatedBy { get; }
    }
}
