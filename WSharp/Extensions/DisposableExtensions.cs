using System;

namespace WSharp.Extensions
{
    /// <summary>Extension methods for the <see cref="IDisposable"/> interface.</summary>
    public static class DisposableExtensions
    {
        /// <summary>Tries to dispose the <see cref="IDisposable"/>.</summary>
        /// <param name="disposable"><see cref="IDisposable"/> to dispose.</param>
        /// <param name="e">The exception if one occured.</param>
        /// <returns><see langword="true"/>: Succesfully disposed.</returns>
        public static bool TryDispose(this IDisposable disposable, out Exception e)
        {
            try
            {
                disposable.Dispose();
            }
            catch (Exception ex)
            {
                e = ex;
                return false;
            }

            e = null;
            return true;
        }
    }
}