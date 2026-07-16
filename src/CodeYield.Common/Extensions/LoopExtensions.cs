using CodeYield.Common.Collections;

namespace CodeYield.Common.Extensions
{
    /// <summary>
    /// Extension methods that enable <c>.AsLoop()</c> on any <see cref="IEnumerable{T}"/>
    /// to provide rich iteration metadata via <see cref="LoopContext{T}"/>.
    /// </summary>
    public static class LoopExtensions
    {
        /// <summary>
        /// Wraps any enumerable with <see cref="LoopContext{T}"/> iteration metadata.
        /// Calling <c>Console.WriteLine</c> on the result displays <c>[item1, item2, ...]</c>.
        /// </summary>
        public static LoopContextCollection<T> AsLoop<T>(this IEnumerable<T> source)
        {
            var list = source as CyList<T> ?? new CyList<T>(source);
            return list.GetLoopContext();
        }

        /// <summary>
        /// Returns <see cref="LoopContext{T}"/> iteration metadata for a <see cref="CyList{T}"/>.
        /// </summary>
        public static LoopContextCollection<T> AsLoop<T>(this CyList<T> source)
        {
            return source.GetLoopContext();
        }
    }
}
