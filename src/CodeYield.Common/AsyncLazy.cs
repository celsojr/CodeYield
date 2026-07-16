namespace CodeYield.Common
{
    /// <summary>
    /// A thread-safe lazy initializer for asynchronous factory methods.
    /// The factory is invoked at most once; subsequent accesses await the same task.
    /// </summary>
    /// <typeparam name="T">The type of the lazily-initialized value.</typeparam>
    public sealed class AsyncLazy<T>
    {
        private readonly Lazy<Task<T>> _inner;

        /// <summary>Initializes a new instance with the specified asynchronous factory.</summary>
        public AsyncLazy(Func<Task<T>> factory)
        {
            _inner = new Lazy<Task<T>>(
                () => Task.Run(factory),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>Gets the lazily-initialized value.</summary>
        public Task<T> Value => _inner.Value;

        /// <summary>Returns true if the value has been created.</summary>
        public bool IsValueCreated => _inner.IsValueCreated;

        /// <summary>Awaits the value if not yet created; otherwise returns the cached result.</summary>
        public Task<T> GetValueAsync() => Value;
    }
}
