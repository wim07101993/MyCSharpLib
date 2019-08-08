using MyCSharpLib.Services.Serialization.Extensions;
using System;
using System.Collections;
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
        
        public static IEnumerable<T> WhereStringContains<T>(this IEnumerable<T> source, Func<T, string> selector, string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? source
                : source.Where(x => selector(x)?.Contains(value) == true);
        }

        public static IEnumerable<T> WhereCollectionContainsString<T>(this IEnumerable<T> source, Func<T, IEnumerable> selector, string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? source
                : source.Where(x => selector(x)?.Cast<object>().Any(o => o?.SerializeJson().Contains(value) == true) == true);
        }

        public static IEnumerable<T> WhereBetweenOrEqual<T>(this IEnumerable<T> source, Func<T, IComparable> selector, object upper, object lower)
        {
            if (lower == null && upper == null)
                return source;

            if (lower == null)
                return source.WhereLowerOrEqual(selector, upper);
            if (upper == null)
                return source.WhereGreaterOrEqual(selector, lower);

            return source.Where(x =>
                {
                    var element = selector(x);
                    return element != null && element.CompareTo(lower) >= 0 && element.CompareTo(upper) <= 0;
                });
        }

        public static IEnumerable<T> WhereGreaterOrEqual<T>(this IEnumerable<T> source, Func<T, IComparable> selector, object lower)
        {
            if (lower == null)
                return source;
            
            return source.Where(x =>
            {
                var element = selector(x);
                return element != null && element.CompareTo(lower) >= 0;
            });
        }

        public static IEnumerable<T> WhereLowerOrEqual<T>(this IEnumerable<T> source, Func<T, IComparable> selector, object upper)
        {
            if (upper == null)
                return source;
            
            return source.Where(x =>
            {
                var element = selector(x);
                return element != null && element.CompareTo(upper) <= 0;
            });
        }
    }
}
