using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCSharpLib.Extensions
{
    public static class EnumerableExtensions
    {
        public static void Add<T>(this IList<T> source, IEnumerable<T> items)
        {
            if (source == null)
                throw new NullReferenceException(nameof(source));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
                source.Add(item);
        }

        public static IEnumerable<T> Combine<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            if (source == null)
                throw new NullReferenceException(nameof(source));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var item in source)
                yield return item;
            foreach (var item in items)
                yield return item;
        }

        public static string ToAsciiString(this IEnumerable<byte> bytes)
        {
            var array = bytes.ToArray();
            return Encoding.ASCII.GetString(array, 0, array.Length);
        }
    }
}
