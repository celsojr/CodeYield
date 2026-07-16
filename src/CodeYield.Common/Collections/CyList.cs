namespace CodeYield.Common.Collections
{
    /// <summary>
    /// A list wrapper that provides rich iteration metadata through <see cref="GetLoopContext"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// var items = new CyList&lt;string&gt; { "Apple", "Banana", "Orange" };
    /// foreach (var loop in items.GetLoopContext())
    /// {
    ///     Console.WriteLine($"{loop.Index}: {loop.Item} (First: {loop.IsFirst}, Last: {loop.IsLast})");
    /// }
    /// </code>
    /// </example>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public sealed class CyList<T> : List<T>
    {
        /// <summary>Initializes a new empty instance of <see cref="CyList{T}"/>.</summary>
        public CyList() : base() { }

        /// <summary>Initializes a new instance containing elements from the specified collection.</summary>
        public CyList(IEnumerable<T> collection) : base(collection) { }

        /// <summary>Initializes a new instance with the specified initial capacity.</summary>
        public CyList(int capacity) : base(capacity) { }

        /// <summary>
        /// Iterates over the list, yielding a <see cref="LoopContext{T}"/> for each element
        /// that provides index, position, and navigation metadata.
        /// </summary>
        public IEnumerable<LoopContext<T>> GetLoopContext()
        {
            int count = this.Count;
            int index = 0;

            foreach (var item in this)
            {
                yield return new LoopContext<T>(
                    item,
                    this,
                    index,
                    index == 0,
                    index == count - 1,
                    index % 2 == 0,
                    index % 2 != 0,
                    count,
                    count - index - 1
                );
                index++;
            }
        }
    }
}
