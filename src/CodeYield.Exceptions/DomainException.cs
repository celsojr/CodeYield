namespace CodeYield.Exceptions
{
    /// <summary>
    /// Base exception for all domain-level errors. Throw this or a derived type
    /// when a business rule or invariant has been violated.
    /// </summary>
    public abstract class DomainException : Exception
    {
        /// <summary>Initializes a new <see cref="DomainException"/> with the specified message.</summary>
        protected DomainException(string message) : base(message) { }

        /// <summary>Initializes a new <see cref="DomainException"/> with the specified message and inner exception.</summary>
        protected DomainException(string message, Exception inner) : base(message, inner) { }
    }
}
