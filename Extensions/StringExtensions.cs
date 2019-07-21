using System.Text;

namespace MyCSharpLib.Extensions
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
    }
}
