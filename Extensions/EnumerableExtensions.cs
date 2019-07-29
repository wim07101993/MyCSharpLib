using System;
using System.Collections.Generic;

namespace MyCSharpLib.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Add<T>(this IEnumerable<T> source, IEnumerable<T> items)
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
