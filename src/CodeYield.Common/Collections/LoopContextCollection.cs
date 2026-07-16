using System.Collections;

namespace CodeYield.Common.Collections
{
    /// <summary>
    /// A materialized collection of <see cref="LoopContext{T}"/> items that supports
    /// enumeration, indexing, and bracket-delimited display.
    /// </summary>
    /// <example>
    /// <code>
    /// var numbers = new List&lt;int&gt; { 1, 2, 3 };
    /// Console.WriteLine(numbers.AsLoop()); // [1, 2, 3]
    /// </code>
    /// </example>
    /// <typeparam name="T">The element type.</typeparam>
    public sealed class LoopContextCollection<T> : IEnumerable<LoopContext<T>>
    {
        private readonly IReadOnlyList<LoopContext<T>> _contexts;

        /// <summary>Initializes a new instance with the specified contexts.</summary>
        internal LoopContextCollection(IReadOnlyList<LoopContext<T>> contexts)
        {
            _contexts = contexts;
        }

        /// <summary>Gets the number of elements.</summary>
        public int Count => _contexts.Count;

        /// <summary>Gets the context at the specified index.</summary>
        public LoopContext<T> this[int index] => _contexts[index];

        /// <summary>Returns the items as a bracket-delimited, comma-separated list.</summary>
        public override string ToString() =>
            $"[{string.Join(", ", _contexts.Select(c => c.Item))}]";

        /// <inheritdoc />
        public IEnumerator<LoopContext<T>> GetEnumerator() => _contexts.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
