using NSubstitute;
using pbdq.Tests.Selenium.FX.Helpers;
using pbdq.Tests.Selenium.FX.Helpers.Validators;
using Shouldly;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.XPathBuilderTests
{
    public class XPathBuilder_GetTextPart_Tests
    {
        private readonly XPathBuilder _builder;
        private readonly IXPathValidator _xPathValidator;
        private readonly ICssValidator _cssValidator;

        public XPathBuilder_GetTextPart_Tests()
        {
            _xPathValidator = Substitute.For<IXPathValidator>();
            _cssValidator = Substitute.For<ICssValidator>();
            _builder = new XPathBuilder(_xPathValidator, _cssValidator);
        }

        [Theory]
        [InlineData(null, "[text()]")]
        [InlineData("a", "[text()='a']")]
        [InlineData("6", "[text()='6']")]
        [InlineData("abc", "[text()='abc']")]
        [InlineData("a c", "[text()='a c']")]
        [InlineData("ABC", "[text()='ABC']")]
        [InlineData("abc:def", "[text()='abc:def']")]
        [InlineData("名字", "[text()='名字']")]
        [InlineData("abc \"def\"", "[text()='abc \"def\"']")]
        [InlineData("abc 'def'", "[text()='abc &apos;def&apos;']")]
        [InlineData("x > 9", "[text()='x &gt; 9']")]
        public void when_creating_text_with_valid_values(string text, string expectedResult)
        {
            var result = _builder.GetTextPart(text);
            result.ShouldBe(expectedResult);

            _cssValidator.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());

            _xPathValidator.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateQName(Arg.Any<string>(), Arg.Any<string>());
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(null, "[text()]")]
        [InlineData("a", "[contains(text(),'a')]")]
        [InlineData("6", "[contains(text(),'6')]")]
        [InlineData("abc", "[contains(text(),'abc')]")]
        [InlineData("a c", "[contains(text(),'a c')]")]
        [InlineData("ABC", "[contains(text(),'ABC')]")]
        [InlineData("abc:def", "[contains(text(),'abc:def')]")]
        [InlineData("名字", "[contains(text(),'名字')]")]
        [InlineData("abc \"def\"", "[contains(text(),'abc \"def\"')]")]
        [InlineData("abc 'def'", "[contains(text(),'abc &apos;def&apos;')]")]
        [InlineData("x > 9", "[contains(text(),'x &gt; 9')]")]
        public void when_creating_partial_text_with_valid_values(string text, string expectedResult)
        {
            var result = _builder.GetTextPart(text, true);
            result.ShouldBe(expectedResult);

            _cssValidator.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());

            _xPathValidator.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateQName(Arg.Any<string>(), Arg.Any<string>());
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}