using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSharp.Extensions
{
    /// <summary>
    ///     Extension methods for the <see cref="IEnumerable"/> and <see cref="IEnumerable{T}"/> interfaces.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>Converts a collection of bytes to a string using ASCII.</summary>
        /// <param name="bytes">Bytes to convert.</param>
        /// <returns>The string representation of the bytes.</returns>
        public static string ToAsciiString(this IEnumerable<byte> bytes)
        {
            var array = bytes.ToArray();
            return Encoding.ASCII.GetString(array, 0, array.Length);
        }

        /// <summary>Converts a collection of bytes to a string using UTF8.</summary>
        /// <param name="bytes">Bytes to convert.</param>
        /// <returns>The string representation of the bytes.</returns>
        public static string ToUtf8String(this IEnumerable<byte> bytes)
        {
            var array = bytes.ToArray();
            return Encoding.UTF8.GetString(array, 0, array.Length);
        }

        /// <summary>
        ///     Searches for all elements that have a given value in a given string property.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the collection.</typeparam>
        /// <param name="source">The collection to search in.</param>
        /// <param name="selector">The selector to select the string property to search in.</param>
        /// <param name="value">The value that the string property should contain.</param>
        /// <returns>The filtered collection.</returns>
        public static IEnumerable<T> WhereStringContains<T>(this IEnumerable<T> source, Func<T, string> selector, string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? source
                : source.Where(x => selector(x)?.Contains(value) == true);
        }

        /// <summary>
        ///     Searches for all elements that have a enumerable property that contains an element
        ///     that contains a string value. The check happens by serializing the elements in the
        ///     enumerable property using json and comparing the result to the given value.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the collection.</typeparam>
        /// <param name="source">The collection to search in.</param>
        /// <param name="selector">
        ///     The selector to select the enumerable property to search in.
        /// </param>
        /// <param name="value">The value that an enumerable properties element should contain.</param>
        /// <returns>The filtered collection.</returns>
        public static IEnumerable<T> WhereCollectionContainsString<T>(this IEnumerable<T> source, Func<T, IEnumerable> selector, string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? source
                : source.Where(x => selector(x)?.Cast<object>().Any(o => o?.SerializeJson().Contains(value) == true) == true);
        }

        /// <summary>
        ///     Searches for all elements which values are between an upper and lower bound.
        ///     <para>If the upper or lower bound is null, it is ignored.</para>
        /// </summary>
        /// <typeparam name="T">Type of the elements in the collection.</typeparam>
        /// <param name="source">The collection to search in.</param>
        /// <param name="selector">The selector to select the property to compare.</param>
        /// <param name="upper">The upper bound.</param>
        /// <param name="lower">The lower bound.</param>
        /// <returns>The elements that are between the boundries.</returns>
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

        /// <summary>
        ///     Searches for all elements which values are greater compared to the lower bound.
        ///     <para>If the bound is null, it is ignored.</para>
        /// </summary>
        /// <typeparam name="T">Type of the elements in the collection.</typeparam>
        /// <param name="source">The collection to search in.</param>
        /// <param name="selector">The selector to select the property to compare.</param>
        /// <param name="lower">The lower bound</param>
        /// <returns>The elements that are greater then the lower bound.</returns>
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

        /// <summary>
        ///     Searches for all elements which values are lower compared to the lower bound.
        ///     <para>If the bound is null, it is ignored.</para>
        /// </summary>
        /// <typeparam name="T">Type of the elements in the collection.</typeparam>
        /// <param name="source">The collection to search in.</param>
        /// <param name="selector">The selector to select the property to compare.</param>
        /// <param name="lower">The lower bound</param>
        /// <returns>The elements that are smaller then the lower bound.</returns>
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

        /// <summary>Does an action for each of the items in a collection.</summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The collection to perform the action on.</param>
        /// <param name="action">The action to perform on all the elements of the collection.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }
    }
}