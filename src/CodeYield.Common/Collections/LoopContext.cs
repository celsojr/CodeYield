namespace CodeYield.Common.Collections
{
    /// <summary>
    /// Provides rich iteration metadata for a single element within a collection,
    /// including index, position flags, and navigation properties.
    /// </summary>
    /// <typeparam name="T">The type of the element being iterated.</typeparam>
    public sealed class LoopContext<T>
    {
        private readonly IList<T> _source;

        /// <summary>Gets the current element.</summary>
        public T Item { get; }

        /// <summary>Gets the zero-based index of the current element.</summary>
        public int Index { get; }

        /// <summary>Gets whether this is the first element in the collection.</summary>
        public bool IsFirst { get; }

        /// <summary>Gets whether this is the last element in the collection.</summary>
        public bool IsLast { get; }

        /// <summary>Gets whether the current index is even.</summary>
        public bool IsEven { get; }

        /// <summary>Gets whether the current index is odd.</summary>
        public bool IsOdd { get; }

        /// <summary>Gets the total number of elements in the collection.</summary>
        public int Count { get; }

        /// <summary>Gets the number of elements remaining after the current element.</summary>
        public int Left { get; }

        /// <summary>Gets the 1-based position of the current element.</summary>
        public int Step => Index + 1;

        /// <summary>Gets the zero-based index from the end of the collection.</summary>
        public int IndexFromEnd => Count - Index - 1;

        /// <summary>Gets whether the collection contains exactly one element.</summary>
        public bool IsSingle => Count == 1;

        /// <summary>Gets whether the collection is empty.</summary>
        public bool IsEmpty => Count == 0;

        /// <summary>Gets the first element in the collection, or <c>default</c> if empty.</summary>
        public T? First => Count > 0 ? _source[0] : default;

        /// <summary>Gets the last element in the collection, or <c>default</c> if empty.</summary>
        public T? Last => Count > 0 ? _source[Count - 1] : default;

        /// <summary>Gets the next element in the collection, or <c>default</c> if this is the last.</summary>
        public T? Next => Index < Count - 1 ? _source[Index + 1] : default;

        /// <summary>Gets the previous element in the collection, or <c>default</c> if this is the first.</summary>
        public T? Prev => Index > 0 ? _source[Index - 1] : default;

        internal LoopContext(T item, IList<T> source, int index, bool isFirst, bool isLast,
                            bool isEven, bool isOdd, int count, int left)
        {
            Item = item;
            _source = source;
            Index = index;
            IsFirst = isFirst;
            IsLast = isLast;
            IsEven = isEven;
            IsOdd = isOdd;
            Count = count;
            Left = left;
        }
    }
}
