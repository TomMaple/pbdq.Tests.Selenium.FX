using System;
using NSubstitute;
using pbdq.Tests.Selenium.FX.Helpers;
using pbdq.Tests.Selenium.FX.Helpers.Validators;
using Shouldly;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.XPathBuilderTests
{
    public class XPathBuilder_GetAttributePart_Tests
    {
        private readonly XPathBuilder _builder;
        private readonly IXPathValidator _xPathValidatorMock;
        private readonly ICssValidator _cssValidatorMock;
        private readonly IXPathValidator _xPathValidator;

        public XPathBuilder_GetAttributePart_Tests()
        {
            _xPathValidator = new XPathValidator();
            _xPathValidatorMock = Substitute.For<IXPathValidator>();
            _xPathValidatorMock.IsReservedFunctionName(Arg.Any<string>())
                .Returns(x => _xPathValidator.IsReservedFunctionName(x.Arg<string>()));
            _xPathValidatorMock.When(x => x.ValidateQName(Arg.Any<string>(), Arg.Any<string>()))
                .Do(x => _xPathValidator.ValidateQName(x.ArgAt<string>(0), x.ArgAt<string>(1)));
            _xPathValidatorMock.When(x => x.ValidateNCName(Arg.Any<string>(), Arg.Any<string>()))
                .Do(x => _xPathValidator.ValidateNCName(x.ArgAt<string>(0), x.ArgAt<string>(1)));

            _cssValidatorMock = Substitute.For<ICssValidator>();

            _builder = new XPathBuilder(_xPathValidatorMock, _cssValidatorMock);
        }

        #region valid input data

        [Theory]
        [InlineData(null, "[@*]")]
        [InlineData("", "[@*='']")]
        [InlineData("en", "[@*='en']")]
        public void when_creating_xpath_attribute_with_null_attribute_name(string attributeValue, string expectedResult)
        {
            var result = _builder.GetAttributePart(null, attributeValue);
            result.ShouldBe(expectedResult);

            _cssValidatorMock.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());

            _xPathValidatorMock.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());
            _xPathValidatorMock.DidNotReceiveWithAnyArgs().ValidateQName(Arg.Any<string>(), Arg.Any<string>());
            _xPathValidatorMock.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData("lang", null, "[@lang]")]
        [InlineData("lang", "", "[@lang='']")]
        [InlineData("lang", "en", "[@lang='en']")]
        [InlineData("a", "5", "[@a='5']")]
        [InlineData("STYLE", null, "[@STYLE]")]
        [InlineData("abc:class", null, "[@abc:class]")]
        [InlineData("main-lang", "en", "[@main-lang='en']")]
        [InlineData("_lang", "en", "[@_lang='en']")]
        [InlineData("名字", null, "[@名字]")]
        public void when_creating_xpath_attribute_with_valid_values(string attributeName, string attributeValue, string expectedResult)
        {
            var result = _builder.GetAttributePart(attributeName, attributeValue);
            result.ShouldBe(expectedResult);

            _cssValidatorMock.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());

            _xPathValidatorMock.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());
            _xPathValidatorMock.Received(1).ValidateQName(attributeName, "Attribute Name");
            _xPathValidatorMock.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void when_creating_xpath_attribute_with_reserved_name()
        {
            var result = _builder.GetAttributePart("text", "text");
            result.ShouldBe("[@text='text']");

            _cssValidatorMock.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());

            _xPathValidatorMock.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());
            _xPathValidatorMock.Received(1).ValidateQName("text", "Attribute Name");
            _xPathValidatorMock.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData("x > 11", "[@attrName='x &gt; 11']")]
        [InlineData("<12", "[@attrName='&lt;12']")]
        [InlineData("A&B", "[@attrName='A&amp;B']")]
        [InlineData("McGregor's", "[@attrName='McGregor&apos;s']")]
        public void when_creating_xpath_attribute_with_value_to_be_encoded(string attributeValue, string expectedResult)
        {
            var result = _builder.GetAttributePart("attrName", attributeValue);
            result.ShouldBe(expectedResult);

            _cssValidatorMock.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());

            _xPathValidatorMock.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());
            _xPathValidatorMock.Received(1).ValidateQName("attrName", "Attribute Name");
            _xPathValidatorMock.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());
        }

        #endregion

        #region invalid input data

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("abc")]
        public void when_creating_xpath_attribute_with_invalid_attibute_name(string attributeValue)
        {
            _xPathValidatorMock.When(x => x.ValidateQName("invalid-tag", Arg.Any<string>()))
                .Do(x => throw new ArgumentException("Exception message."));

            var exception = Should.Throw<ArgumentException>(() => _builder.GetAttributePart("invalid-tag", attributeValue));

            exception.ShouldNotBeNull();
            exception.Message.ShouldBe("Exception message.");

            _cssValidatorMock.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());

            _xPathValidatorMock.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());
            _xPathValidatorMock.Received(1).ValidateQName("invalid-tag", "Attribute Name");
            _xPathValidatorMock.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());
        }

        #endregion
    }
}