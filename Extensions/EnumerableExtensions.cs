using System;
using System.Collections.Generic;

namespace MyCSharpLib.Extensions
{
    public static class EnumerableExtensions
    {
        public static void Add<T>(this IList<T> source, IEnumerable<T> items)
        {
            if (source == null)
                throw new NullReferenceException(nameof(source));
            if (items == null)
                throw new NullReferenceException(nameof(source));

            foreach (var item in items)
                source.Add(item);
        }

        public static IEnumerable<T> Combine<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            if (source == null)
                throw new NullReferenceException(nameof(source));
            if (items == null)
                throw new NullReferenceException(nameof(source));

            foreach (var item in source)
                yield return item;
            foreach (var item in items)
                yield return item;
        }
    }
}
