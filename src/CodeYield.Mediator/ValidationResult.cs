namespace CodeYield.Mediator
{
    /// <summary>
    /// Outcome of a validation step. <see cref="IsValid"/> is true when all rules pass.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>Gets whether validation passed.</summary>
        public bool IsValid { get; }

        /// <summary>Gets the list of error messages when validation fails.</summary>
        public IReadOnlyList<string> Errors { get; }

        private ValidationResult(bool isValid, IReadOnlyList<string> errors)
        {
            IsValid = isValid;
            Errors = errors;
        }

        /// <summary>Creates a successful validation result.</summary>
        public static ValidationResult Success() => new(true, Array.Empty<string>());

        /// <summary>Creates a failed validation result with the specified errors.</summary>
        public static ValidationResult Failure(params string[] errors) => new(false, errors);
    }
}
