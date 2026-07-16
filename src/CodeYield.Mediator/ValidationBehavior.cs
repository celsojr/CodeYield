using Microsoft.Extensions.DependencyInjection;

namespace CodeYield.Mediator
{
    /// <summary>
    /// Validates a command or query using a registered <see cref="IValidator{T}"/>
    /// before it reaches the handler. Short-circuits the pipeline by returning
    /// <see cref="Result{T}.Failure"/> when validation fails.
    /// </summary>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>Initializes a new instance of <see cref="ValidationBehavior{TRequest, TResponse}"/>.</summary>
        public ValidationBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken ct, Func<Task<TResponse>> next)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(typeof(TRequest));
            var validator = _serviceProvider.GetService(validatorType);

            if (validator is null)
                return await next();

            var validateMethod = validatorType.GetMethod(nameof(IValidator<TRequest>.ValidateAsync))!;
            var task = (Task<ValidationResult>)validateMethod.Invoke(validator, new object?[] { request, ct })!;
            var result = await task;

            if (result.IsValid)
                return await next();

            // Build a Result<TResponse>.Failure via reflection
            var failureMethod = typeof(Result<>)
                .MakeGenericType(typeof(TResponse))
                .GetMethods()
                .First(m => m.Name == "Failure" && m.GetParameters().Length == 1);

            return (TResponse)failureMethod.Invoke(null, new object[] { string.Join("; ", result.Errors) })!;
        }
    }
}
