using System;

namespace MyCSharpLib.Extensions
{
    public static class DisposableExtensions
    {
        public static void TryDispose(this IDisposable disposable)
        {
            try
            {
                disposable.Dispose();
            }
            catch
            {
            }
        }
    }
}
