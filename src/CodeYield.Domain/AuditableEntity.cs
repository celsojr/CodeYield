using CodeYield.Abstractions;

namespace CodeYield.Domain
{
    /// <summary>
    /// Base class for entities with audit tracking (creation and update timestamps and user identifiers).
    /// </summary>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    public abstract class AuditableEntity<TId> : BaseEntity<TId>, IEditableEntity
    {
        /// <inheritdoc />
        public DateTimeOffset CreatedOnUtc { get; protected set; }

        /// <inheritdoc />
        public string? CreatedBy { get; protected set; }

        /// <inheritdoc />
        public DateTimeOffset? UpdatedOnUtc { get; protected set; }

        /// <inheritdoc />
        public string? UpdatedBy { get; protected set; }

        /// <summary>Sets the audit fields when the entity is first created.</summary>
        protected void SetCreated(DateTimeOffset onUtc, string? by)
        {
            CreatedOnUtc = onUtc;
            CreatedBy = by;
        }

        /// <summary>Sets the audit fields when the entity is updated.</summary>
        protected void SetUpdated(DateTimeOffset onUtc, string? by)
        {
            UpdatedOnUtc = onUtc;
            UpdatedBy = by;
        }
    }
}
