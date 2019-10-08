using NUnit.Framework;
using WSharp.Extensions;
using FluentAssertions;
using System.Security;
using WSharp.Tests.Mocks;

namespace WSharp.Tests.Tests.Extensions
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

        [Test]
        public void GetValue()
        {
            var secure = new SecureString();
            secure.AppendChar('h');
            secure.AppendChar('e');
            secure.AppendChar('l');
            secure.AppendChar('l');
            secure.AppendChar('o');
            secure.AppendChar(' ');
            secure.AppendChar('w');
            secure.AppendChar('o');
            secure.AppendChar('r');
            secure.AppendChar('l');
            secure.AppendChar('d');

            secure.GetValue()
                .Should()
                .Be("hello world", "that is what is stored in the stirng.");
        }

        [Test]
        public void BuildSelectExpressionSingleProperty()
        {
            var obj = new ValidationObject { GreaterThen5 = 5 };
            var propertyPath = nameof(ValidationObject.GreaterThen5);
            var exp = propertyPath.BuildSelectExpression<ValidationObject>();
            exp.Compile()(obj)
                .Should()
                .Be(5, "that is the value of the requested property");
        }

        [Test]
        public void BuildSelectExpressionNestedProeprty()
        {
            var obj = new ValidationObject { Nested = new NestedValidationObject { Different = 6 } };
            var propertyPath = $"{nameof(ValidationObject.Nested)}/{nameof(NestedValidationObject.Different)}";
            var exp = propertyPath.BuildSelectExpression<ValidationObject>();
            exp.Compile()(obj)
                .Should()
                .Be(6, "that is the value of the requested property");
        }
    }
}
