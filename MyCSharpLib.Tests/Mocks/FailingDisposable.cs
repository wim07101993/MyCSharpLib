using System;

namespace MyCSharpLib.Tests.Mocks
{
    public class FailingDisposable : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
