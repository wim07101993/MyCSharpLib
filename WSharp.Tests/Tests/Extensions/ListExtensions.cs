using NUnit.Framework;
using WSharp.Extensions;
using System.Collections.Generic;
using FluentAssertions;

namespace WSharp.Tests.Tests.Extensions
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
                .Be(-1, "there is no iten in the collection that is 'doesn't exist'");
        }

        [Test]
        public void IndexWhereNull()
        {
            new List<string> { "hello", "world", "this", "is", "another", "test" }
                .IndexesWhere(null)
                .Should()
                .BeEquivalentTo(new[] { 0, 1, 2, 3, 4, 5 }, "when there is no predicate defined, all the indexes if the list should be returned.");

            new List<string>()
                .IndexesWhere(null)
                .Should()
                .BeNull("when there is no predicate defined and the list is 'null', there are no indexes to return");
        }

        [Test]
        public void IndexesWhere()
        {
            var list = new List<string> { "hello", "world", "this", "is", "another", "test" };
            
            list.IndexesWhere(x => x.Contains('i'))
                .Should()
                .BeEquivalentTo(new[] { 2, 3 }, "all the indexes of the items that contain an 'i' should be returned");

            list.IndexesWhere(x => x == "doesn't exist")
                .Should()
                .BeNull("there is no iten in the collection that is 'doesn't exist'");
        }

        [Test]
        public void IndexOfLastNullPredicate()
        {
            new List<string> { "hello", "world", "this", "is", "another", "test" }
                .IndexOfLast(null)
                .Should()
                .Be(5, "when there is no predicate defined, the method should return the index of the last item (0 if the list contains any items)");

            new List<string>()
                .IndexOfLast(null)
                .Should()
                .Be(-1, "when there is no predicate defined, the method should return -1 if there are not items in the collection");
        }

        [Test]
        public void IndexOfLast()
        {
            var list = new List<string> { "hello", "world", "this", "is", "another", "test" };


            list.IndexOfLast(x => x.Contains('i'))
                .Should()
                .Be(3, "the last item that contains an 'i' is 'this' which is in the third position (index 2)");

            list.IndexOfLast(x => x == "doesn't exist")
                .Should()
                .Be(-1, "there is no iten in the collection that is 'doesn't exist'");
        }

        #endregion FINDING


        #region REMOVING

        [Test]
        public void RemoveFirstNull()
        {
            var list = new List<string> { "hello", "world", "this", "is", "another", "test" };

            list.RemoveFirst(null)
                .Should()
                .BeTrue("The method should return true when an items was removed");

            list.Should()
                .NotContain("hello", "that was the first item. It should be removed")
                .And
                .HaveCount(5, "only one item should be removed");

            new List<string>()
                .RemoveFirst(null)
                .Should()
                .BeFalse("if there are not items in the list, none can be remove and so the method should return false.");
        }

        [Test]
        public void RemoveFirst()
        {
            var list = new List<string> { "hello", "world", "this", "is", "another", "test" };

            list.RemoveFirst(x => x.Contains('i'))
                .Should()
                .BeTrue("there is an item that contains the letter 'i'");

            list.Should()
                .NotContain("this", "that was the first item that contains an 'i'")
                .And
                .HaveCount(5, "only one item should be removed");

            list = new List<string> { "hello", "world", "this", "is", "another", "test" };

            list.RemoveFirst(x => x == "doesn't exist")
                .Should()
                .BeFalse("there is no item that is 'doesn't exist'");

            list.Should()
                .HaveCount(6);
                
        }

        [Test]
        public void RemoveWhereNull()
        {
            var list = new List<string> { "hello", "world", "this", "is", "another", "test" };

            list.RemoveWhere(null)
                .Should()
                .BeTrue("The method should return true when an items was removed");

            list.Should()
                .HaveCount(0, "all the items should have been removed");

            new List<string>()
                .RemoveWhere(null)
                .Should()
                .BeFalse("if there are not items in the list, none can be remove and so the method should return false.");
        }

        [Test]
        public void RemoveWhere()
        {
            var list = new List<string> { "hello", "world", "this", "is", "another", "test" };

            list.RemoveWhere(x => x.Contains('i'))
                .Should()
                .BeTrue("there is an item that contains the letter 'i'");

            list.Should()
                .NotContain("this", "that was the first item that contains an 'i'")
                .And
                .NotContain("is", "that was the other item that contains an 'i'")
                .And
                .HaveCount(4, "two items should be removed");

            list = new List<string> { "hello", "world", "this", "is", "another", "test" };

            list.RemoveWhere(x => x == "doesn't exist")
                .Should()
                .BeFalse("there is no item that is 'doesn't exist'");

            list.Should()
                .HaveCount(6);

        }

        [Test]
        public void RemoveLastNull()
        {
            var list = new List<string> { "hello", "world", "this", "is", "another", "test" };

            list.RemoveLast(null)
                .Should()
                .BeTrue("The method should return true when an items was removed");

            list.Should()
                .NotContain("test", "that was the first item. It should be removed")
                .And
                .HaveCount(5, "only one item should be removed");

            new List<string>()
                .RemoveLast(null)
                .Should()
                .BeFalse("if there are not items in the list, none can be remove and so the method should return false.");
        }

        [Test]
        public void RemoveLast()
        {
            var list = new List<string> { "hello", "world", "this", "is", "another", "test" };

            list.RemoveLast(x => x.Contains('i'))
                .Should()
                .BeTrue("there is an item that contains the letter 'i'");

            list.Should()
                .NotContain("is", "that was the last item that contains an 'i'")
                .And
                .HaveCount(5, "only one item should be removed");

            list = new List<string> { "hello", "world", "this", "is", "another", "test" };

            list.RemoveLast(x => x == "doesn't exist")
                .Should()
                .BeFalse("there is no item that is 'doesn't exist'");

            list.Should()
                .HaveCount(6);

        }

        #endregion REMOVING
    }
}
