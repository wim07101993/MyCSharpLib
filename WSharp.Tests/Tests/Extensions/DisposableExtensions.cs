using FluentAssertions;
using WSharp.Extensions;
using WSharp.Tests.Mocks;
using NUnit.Framework;
using System;

namespace WSharp.Tests.Tests.Extensions
{
    public class DisposableExtensions
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void TryDisposeWithException()
        {
            new FailingDisposable()
                .Invoking(x => x.Dispose())
                .Should()
                .Throw<NotImplementedException>("the test is designed to");

            new FailingDisposable()
               .Invoking(x => x.TryDispose())
               .Should()
               .NotThrow<NotImplementedException>("the try dispose should never throw an exception");
        }

        [Test]
        public void TryDisposeWithoutException()
        {
            new SuccedingDisposable()
                .Invoking(x => x.Dispose())
                .Should()
                .NotThrow<NotImplementedException>("the test is designed to");

            new SuccedingDisposable()
               .Invoking(x => x.TryDispose())
               .Should()
               .NotThrow<NotImplementedException>("the try dispose should never throw an exception");
        }
    }
}
