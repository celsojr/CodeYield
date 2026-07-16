namespace CodeYield.Mediator
{
    /// <summary>
    /// Validates a command or query before it reaches the handler.
    /// Implementations are resolved by <see cref="ValidationBehavior{TRequest, TResponse}"/>.
    /// This is a lightweight contract — no dependency on FluentValidation or other libraries.
    /// </summary>
    /// <typeparam name="T">The command or query type to validate.</typeparam>
    public interface IValidator<in T>
    {
        /// <summary>Validates the instance and returns the result.</summary>
        ValueTask<ValidationResult> ValidateAsync(T instance, CancellationToken ct = default);
    }
}
