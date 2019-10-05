using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using WSharp.Extensions;
using WSharp.Tests.Mocks;

namespace WSharp.Tests.Tests.Extensions
{
    [TestFixture]
    public class ObjectExtensions
    {
        [Test]
        public void Validate()
        {
            var obj = new ValidationObject
            {
                GreaterThen5 = 10,
                Nested = new NestedValidationObject
                {
                    GreaterThen5 = 10,
                    NotEmpty = "This is not empty",
                    NotNull = new object(),
                },
                NotEmpty = "This is not empty",
                NotNull = new object(),
                List = new List<NestedValidationObject>
                {
                    new NestedValidationObject
                    {
                        GreaterThen5 = 10,
                        NotEmpty = "This is not empty",
                        NotNull = new object(),
                    },new NestedValidationObject
                    {
                        GreaterThen5 = 10,
                        NotEmpty = "This is not empty",
                        NotNull = new object(),
                    },
                }
            };

            obj.Validate(out var errors)
                .Should()
                .BeTrue("the object is valid");

            errors
                .Should()
                .BeEmpty("the object is valid");
        }

        [Test]
        public void ValidateNonValide()
        {
            var obj = new ValidationObject
            {
                GreaterThen5 = 1,
                Nested = new NestedValidationObject
                {
                    GreaterThen5 = 10,
                    NotEmpty = "This is not empty",
                    NotNull = new object(),
                },
                NotEmpty = "This is not empty",
                NotNull = new object(),
                List = new List<NestedValidationObject>
                {
                    new NestedValidationObject
                    {
                        GreaterThen5 = 10,
                        NotEmpty = "This is not empty",
                        NotNull = new object(),
                    },new NestedValidationObject
                    {
                        GreaterThen5 = 10,
                        NotEmpty = "This is not empty",
                        NotNull = new object(),
                    },
                }
            };

            obj.Validate(out var errors)
                .Should()
                .BeFalse();
            errors
                .Should()
                .HaveCount(1, "one property is not valid");

            obj.GreaterThen5 = 6;
            obj.Nested.GreaterThen5 = 1;
            obj.Validate(out errors)
                .Should()
                .BeFalse();
            errors
                .Should()
                .HaveCount(1, "One property is not valid");

            obj.GreaterThen5 = 1;
            obj.NotEmpty = "";
            obj.NotNull = null;
            obj.Nested.NotNull = null;
            obj.Validate(out errors)
                .Should()
                .BeFalse();
            errors
                .Should()
                .HaveCount(5, "Five property is not valid");

            obj.NotEmpty = "Not empty";
            obj.NotNull = new object();
            obj.List[0].NotNull = null;
            obj.Validate(out errors)
                .Should()
                .BeFalse();
            errors
                .Should()
                .HaveCount(4, "Five property is not valid");
        }
    }
}
