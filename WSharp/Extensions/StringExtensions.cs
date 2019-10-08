using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace WSharp.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Converts a string to the ASCII equivalent in bytes
        /// </summary>
        /// <param name="str"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="EncoderFallbackException">
        ///     A fallback occurred (see ~/docs/standard/base-types/character-encoding.md for
        ///     complete explanation) -and- <see cref="Encoding.EncoderFallback" /> is set to
        ///     <see cref="EncoderExceptionFallback" />.
        /// </exception>
        /// <returns>The ASCII equivalent of the string input</returns>
        public static byte[] ToAsciiBytes(this string str) => Encoding.ASCII.GetBytes(str);

        public static string GetValue(this SecureString str)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(str);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        public static Expression<Func<TIn, object>> BuildSelectExpression<TIn>(this IList<string> propertyPath, 
            bool includeProperties= true, bool includeIndexProeprties = true, bool includeEnumerableSearch = true)
        {
            var eParameter = Expression.Parameter(typeof(TIn), "x");

            var type = typeof(TIn);
            var propertyName = propertyPath.First();
            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                if (typeof(IList).IsAssignableFrom(type))
                {

                }
                throw new ArgumentException($"The property {property} was not found on type {type}", nameof(propertyPath));
            }

            Expression eProperty = Expression.Property(eParameter, propertyName);

            for (int i = 1; i < propertyPath.Count; i++)
            {
                type = property.PropertyType;
                propertyName = propertyPath[i];
                property = type.GetProperty(propertyName);
                if (property == null)
                    throw new ArgumentException($"The property {property} was not found on type {type}", nameof(propertyPath));

                eProperty = Expression.Property(eProperty, propertyName);
            }

            eProperty = Expression.Convert(eProperty, typeof(object));
            return Expression.Lambda<Func<TIn, object>>(eProperty, eParameter);
        }

        public static Expression<Func<TIn, object>> BuildSelectExpression<TIn>(this string propertyPath, string delimitor = "/")
        {
            if (string.IsNullOrWhiteSpace(propertyPath))
                throw new ArgumentException($"The property path must be specified", nameof(propertyPath));

            var split = propertyPath.Split(new[] { delimitor }, StringSplitOptions.RemoveEmptyEntries);
            return split.BuildSelectExpression<TIn>();
        }
    }
}
