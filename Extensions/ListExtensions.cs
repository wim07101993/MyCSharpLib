using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCSharpLib.Extensions
{
    public static class ListExtensions
    {
        #region FINDING

        public static int IndexOfFirst<T>(this IList<T> list, Func<T, bool> predicate = null)
        {
            if (predicate == null)
            {
                return list.Count > 0
                    ? 0
                    : -1;
            }

            for (var i = 0; i < list.Count; i++)
                if (predicate(list[i]))
                    return i;
            return -1;
        }

        public static List<int> IndexesWhere<T>(this IList<T> list, Func<T, bool> predicate = null)
        {
            if (predicate == null)
            {
                return list.Count > 0
                    ? System.Linq.Enumerable.Range(0, list.Count).ToList()
                    : null;
            }

            var indexes = new List<int>();

            for (var i = 0; i < list.Count; i++)
                if (predicate(list[i]))
                    indexes.Add(i);

            return indexes.Count == 0
                ? null
                : indexes;
        }

        public static int IndexOfLast<T>(this IList<T> list, Func<T, bool> predicate = null)
        {
            if (predicate == null)
                return list.Count - 1;

            for (var i = list.Count - 1; i >= 0; i--)
                if (predicate(list[i]))
                    return i;
            return -1;
        }

        public static IEnumerable<T> Slice<T>(this IList<T> list, int startIndex, int endIndex)
        {
            for (var i = startIndex; i < endIndex; i++)
                yield return list[i];
        }

        #endregion FINDING


        #region REMOVING

        public static bool RemoveFirst<T>(this IList<T> list, Func<T, bool> predicate = null)
        {
            if (predicate == null)
            {
                list.RemoveAt(0);
                return true;
            }

            var index = list.IndexOfFirst(predicate);
            if (index == -1)
                return false;
            list.RemoveAt(index);
            return true;
        }

        public static bool RemoveWhere<T>(this IList<T> list, Func<T, bool> predicate = null)
        {
            if (predicate == null)
            {
                list.Clear();
                return true;
            }

            var indexes = list.IndexesWhere(predicate);

            if (indexes == null)
                return false;

            foreach (var i in indexes)
                list.RemoveAt(i);

            return true;
        }

        public static bool RemoveLast<T>(this IList<T> list, Func<T, bool> predicate = null)
        {
            if (predicate == null)
            {
                list.RemoveAt(list.Count - 1);
                return true;
            }

            var index = list.IndexOfLast(predicate);
            if (index == -1)
                return false;
            list.RemoveAt(index);
            return true;
        }

        #endregion REMOVING
    }
}
