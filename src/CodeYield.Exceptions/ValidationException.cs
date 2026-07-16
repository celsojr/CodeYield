namespace CodeYield.Exceptions
{
    /// <summary>
    /// Thrown when one or more validation rules fail. Carries a dictionary of
    /// field-level error messages keyed by property or field name.
    /// </summary>
    public class ValidationException : DomainException
    {
        /// <summary>Gets the validation errors grouped by field name.</summary>
        public IReadOnlyDictionary<string, string[]> Errors { get; }

        /// <summary>Initializes a new <see cref="ValidationException"/> with the specified error dictionary.</summary>
        public ValidationException(IReadOnlyDictionary<string, string[]> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}
