using NUnit.Framework;
using MyCSharpLib.Extensions;
using System.Collections.Generic;
using FluentAssertions;

namespace MyCSharpLib.Tests.Tests.Extensions
{
    public class ListExtensions
    {
        #region FINDING

        [Test]
        public void IndexOfFirstNullPredicate()
        {
            new List<string> { "hello", "world", "this", "is", "another", "test" }
                .IndexOfFirst(null)
                .Should()
                .Be(0, "when there is no predicate defined, the method should return the index of the first item (0 if the list contains any items)");

            new List<string>()
                .IndexOfFirst(null)
                .Should()
                .Be(-1, "when there is no predicate defined, the method should return -1 if there are not items in the collection");
        }

        [Test]
        public void IndexOfFirst()
        {
            var list = new List<string> { "hello", "world", "this", "is", "another", "test" };


            list.IndexOfFirst(x => x.Contains('i'))
                .Should()
                .Be(2, "the first item that contains an 'i' is 'this' which is in the third position (index 2)");

            list.IndexOfFirst(x => x == "doesn't exist")
                .Should()
                .Be(-1, "There is no iten in the collection that is 'doesn't exist'");
        }

        #endregion FINDING
    }
}
