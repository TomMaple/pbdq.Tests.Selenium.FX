using System;
using NSubstitute;
using pbdq.Tests.Selenium.FX.Helpers;
using pbdq.Tests.Selenium.FX.Helpers.Validators;
using Shouldly;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.XPathBuilderTests
{
    public class XPathBuilder_GetCssClassPart_Tests
    {
        private readonly XPathBuilder _builder;
        private readonly IXPathValidator _xPathValidator;
        private readonly ICssValidator _cssValidator;

        public XPathBuilder_GetCssClassPart_Tests()
        {
            _xPathValidator = Substitute.For<IXPathValidator>();
            _cssValidator = Substitute.For<ICssValidator>();
            _builder = new XPathBuilder(_xPathValidator, _cssValidator);
        }

        [Theory]
        [InlineData("a", "[contains(concat(' ',normalize-space(@class),' '),' a ')]")]
        [InlineData("LI", "[contains(concat(' ',normalize-space(@class),' '),' LI ')]")]
        [InlineData("button", "[contains(concat(' ',normalize-space(@class),' '),' button ')]")]
        [InlineData("primary-button", "[contains(concat(' ',normalize-space(@class),' '),' primary-button ')]")]
        [InlineData("primary_button", "[contains(concat(' ',normalize-space(@class),' '),' primary_button ')]")]
        [InlineData("书", "[contains(concat(' ',normalize-space(@class),' '),' 书 ')]")]
        public void when_creating_xpath_css_class_with_valid_values(string cssClass, string expectedResult)
        {
            var result = _builder.GetCssClassPart(cssClass);
            result.ShouldBe(expectedResult);

            _cssValidator.Received(1).ValidateClassName(cssClass);

            _xPathValidator.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateQName(Arg.Any<string>(), Arg.Any<string>());
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void when_creating_xpath_css_class_with_null_value()
        {
            var exception = Should.Throw<ArgumentNullException>(() => _builder.GetCssClassPart(null));

            exception.ShouldNotBeNull();
            exception.Message.ShouldStartWith("CSS Class Name cannot be null.");

            _cssValidator.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());

            _xPathValidator.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateQName(Arg.Any<string>(), Arg.Any<string>());
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void when_creating_xpath_css_class_with_invalid_value()
        {
            _cssValidator.When(x => x.ValidateClassName(" invalid value "))
                .Do(x => throw new ArgumentException("Exception message."));

            var exception = Should.Throw<ArgumentException>(() => _builder.GetCssClassPart(" invalid value "));

            exception.ShouldNotBeNull();
            exception.Message.ShouldBe("Exception message.");

            _cssValidator.Received(1).ValidateClassName(" invalid value ");

            _xPathValidator.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateQName(Arg.Any<string>(), Arg.Any<string>());
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}