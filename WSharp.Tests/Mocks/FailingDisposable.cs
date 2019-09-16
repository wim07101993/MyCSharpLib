using System;

namespace WSharp.Tests.Mocks
{
    public class FailingDisposable : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
