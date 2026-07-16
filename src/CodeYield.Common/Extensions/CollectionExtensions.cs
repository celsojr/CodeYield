namespace CodeYield.Common.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ICollection{T}"/> and <see cref="IEnumerable{T}"/>
    /// that are not covered by the standard LINQ or BCL APIs.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>Adds <paramref name="item"/> to the collection only when <paramref name="condition"/> is true.</summary>
        public static void AddIf<T>(this ICollection<T> collection, bool condition, T item)
        {
            if (condition) collection.Add(item);
        }

        /// <summary>Adds <paramref name="item"/> to the collection only when it is not null.</summary>
        public static void AddIfNotNull<T>(this ICollection<T> collection, T? item) where T : class
        {
            if (item is not null) collection.Add(item);
        }

        /// <summary>Adds each element of <paramref name="items"/> to the collection.</summary>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }

        /// <summary>Removes all elements matching the predicate and returns how many were removed.</summary>
        public static int RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            if (collection is List<T> list)
                return list.RemoveAll(item => predicate(item));

            var toRemove = collection.Where(predicate).ToList();
            foreach (var item in toRemove)
                collection.Remove(item);
            return toRemove.Count;
        }

        /// <summary>Executes <paramref name="action"/> for each element in the sequence.</summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        /// <summary>Randomly shuffles the list in-place using the Fisher-Yates algorithm.</summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Shared.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>Returns the index of the first element matching the predicate, or -1 if not found.</summary>
        public static int IndexOf<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>Returns the element with the minimum key, or <c>default</c> if the sequence is empty.</summary>
        public static T? MinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
        {
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext()) return default;

            var minItem = enumerator.Current;
            var minKey = keySelector(minItem);

            while (enumerator.MoveNext())
            {
                var key = keySelector(enumerator.Current);
                if (key.CompareTo(minKey) < 0)
                {
                    minItem = enumerator.Current;
                    minKey = key;
                }
            }

            return minItem;
        }

        /// <summary>Returns the element with the maximum key, or <c>default</c> if the sequence is empty.</summary>
        public static T? MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
        {
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext()) return default;

            var maxItem = enumerator.Current;
            var maxKey = keySelector(maxItem);

            while (enumerator.MoveNext())
            {
                var key = keySelector(enumerator.Current);
                if (key.CompareTo(maxKey) > 0)
                {
                    maxItem = enumerator.Current;
                    maxKey = key;
                }
            }

            return maxItem;
        }
    }
}
