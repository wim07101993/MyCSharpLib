using NUnit.Framework;
using MyCSharpLib.Extensions;
using FluentAssertions;

namespace MyCSharpLib.Tests.Tests.Extensions
{
    public class StringExtensions
    {
        [Test]
        public void ToAsciiBytes()
        {
            "hello world"
                .ToAsciiBytes()
                .Should()
                .BeEquivalentTo(new byte[] { 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100 }, "that is the ascii representation.");
        }
    }
}
