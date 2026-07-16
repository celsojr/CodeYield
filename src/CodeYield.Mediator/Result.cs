namespace CodeYield.Mediator
{
    /// <summary>
    /// Represents the outcome of an operation that does not return a value.
    /// Use <see cref="Result{T}"/> for operations that return data.
    /// </summary>
    public class Result
    {
        /// <summary>Gets whether the operation succeeded.</summary>
        public bool IsSuccess { get; }

        /// <summary>Gets the error message if the operation failed, or null on success.</summary>
        public string? Error { get; }

        protected Result(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>Creates a successful result.</summary>
        public static Result Success() => new(true, null);

        /// <summary>Creates a failed result with the specified error message.</summary>
        public static Result Failure(string error) => new(false, error);
    }

    /// <summary>
    /// Represents the outcome of an operation that returns a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of data returned by the operation.</typeparam>
    public class Result<T> : Result
    {
        /// <summary>Gets the data returned by the operation, or null on failure.</summary>
        public T? Data { get; }

        private Result(bool isSuccess, T? data, string? error) : base(isSuccess, error)
        {
            Data = data;
        }

        /// <summary>Creates a successful result containing the specified data.</summary>
        public static Result<T> Success(T data) => new(true, data, null);

        /// <summary>Creates a failed result with the specified error message.</summary>
        public new static Result<T> Failure(string error) => new(false, default, error);
    }
}
