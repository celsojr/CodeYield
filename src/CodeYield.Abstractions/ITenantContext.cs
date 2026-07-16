namespace CodeYield.Abstractions
{
    /// <summary>
    /// Provides multi-tenancy context for the current request or operation,
    /// including the active tenant and the caller's role within that tenant.
    /// </summary>
    public interface ITenantContext
    {
        /// <summary>Gets the current tenant identifier, or null if outside a tenant scope.</summary>
        Guid? TenantId { get; }

        /// <summary>Gets whether the current caller is the tenant owner.</summary>
        bool IsOwner { get; }

        /// <summary>Gets whether the current caller has super-administrator privileges.</summary>
        bool IsSuperAdmin { get; }

        /// <summary>Gets whether the current caller has administrator privileges within the tenant.</summary>
        bool IsAdmin { get; }
    }
}
