using NUnit.Framework;
using WSharp.Extensions;
using System.Collections.Generic;
using FluentAssertions;
using System;
using System.Linq;

namespace WSharp.Tests.Tests.Extensions
{
    public class EnumerableTestExtensions
    {
        [Test]
        public void AddNull()
        {
            new List<string>()
                .Invoking(x => x.Add(null as IEnumerable<string>))
                .Should()
                .ThrowExactly<ArgumentNullException>("the passed argument was null");
        }

        [Test]
        public void Add()
        {
            var list = new List<string>();

            list.Add(new string[] { "hello", "world" });
            list.Should()
                .HaveCount(2, "there were 2 items added and the count was 0.")
                .And
                .Contain("hello", because: "it was added.")
                .And
                .Contain("world", because: "it was also added.");

            list.Add(new string[0]);
            list.Should()
                .HaveCount(2, "there were no new items added");

            list.Add(new string[] { "this", "is", "another", "test" });
            list.Should()
                .HaveCount(6)
                .And
                .ContainInOrder(new string[] { "hello", "world", "this", "is", "another", "test" }, "these elements were added in this sequence");
        }

        [Test]
        public void Combine()
        {
            var firstList = new List<string> { "hello", "world" };
            var secondList = new List<string> { "this", "is", "another", "test" };

            firstList.Concat(secondList)
                .ToList()
                .Should()
                .HaveCount(6)
                .And
                .ContainInOrder(new[] { "hello", "world" })
                .And
                .ContainInOrder(new[] { "this", "is", "another", "test" });
        }

        [Test]
        public void ToAsciiString()
        {
            new byte[] { 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100 }
                .ToAsciiString()
                .Should()
                .Be("hello world");
        }
    }
}
