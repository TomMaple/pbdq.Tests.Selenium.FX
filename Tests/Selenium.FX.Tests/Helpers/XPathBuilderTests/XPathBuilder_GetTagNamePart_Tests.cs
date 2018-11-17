using System;
using NSubstitute;
using pbdq.Tests.Selenium.FX.Helpers;
using pbdq.Tests.Selenium.FX.Helpers.Validators;
using Shouldly;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.XPathBuilderTests
{
    public class XPathBuilder_GetTagNamePart_Tests
    {
        private readonly XPathBuilder _builder;
        private readonly IXPathValidator _xPathValidator;
        private readonly ICssValidator _cssValidator;

        public XPathBuilder_GetTagNamePart_Tests()
        {
            _xPathValidator = Substitute.For<IXPathValidator>();
            _cssValidator = Substitute.For<ICssValidator>();
            _builder = new XPathBuilder(_xPathValidator, _cssValidator);
        }

        #region valid input data

        [Fact]
        public void when_creating_xpath_tag_with_valid_values()
        {
            var result = _builder.GetTagNamePart("valid:TagName");
            result.ShouldBe("valid:TagName");

            _xPathValidator.Received(1).ValidateQName("valid:TagName", "Tag Name");
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());
            _xPathValidator.Received(1).IsReservedFunctionName("valid:TagName");

            _cssValidator.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());
        }

        [Fact]
        public void when_creating_xpath_local_tag_with_valid_values()
        {
            var result = _builder.GetTagNamePart("validTagName", isLocalName: true);
            result.ShouldBe("*[local-name()='validTagName']");

            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateQName(Arg.Any<string>(), Arg.Any<string>());
            _xPathValidator.Received(1).ValidateNCName("validTagName", "Tag Name");
            _xPathValidator.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());

            _cssValidator.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());
        }

        [Fact]
        public void then_creating_xpath_tag_with_reserved_name()
        {
            _xPathValidator.IsReservedFunctionName("reserved:Name").Returns(true);

            var result = _builder.GetTagNamePart("reserved:Name");
            result.ShouldBe("*[name()='reserved:Name']");

            _xPathValidator.Received(1).ValidateQName("reserved:Name", "Tag Name");
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());
            _xPathValidator.Received(1).IsReservedFunctionName("reserved:Name");

            _cssValidator.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());
        }

        #endregion

        #region invalid input data

        [Fact]
        public void when_creating_xpath_local_tag_with_null_value()
        {
            var exception = Should.Throw<ArgumentNullException>(() => _builder.GetTagNamePart(null, isLocalName: true));

            exception.ShouldNotBeNull();
            exception.Message.ShouldStartWith("Tag Name cannot be null.");

            _xPathValidator.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateQName(Arg.Any<string>(), Arg.Any<string>());
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());

            _cssValidator.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());
        }

        [Fact]
        public void when_creating_xpath_tag_with_invalid_tag_name()
        {
            _xPathValidator.When(x => x.ValidateQName(" invalid:Tag", "Tag Name"))
                .Do(x => throw new ArgumentException("Tag Name contains invalid character “ ” at position 0."));

            var exception = Should.Throw<ArgumentException>(() => _builder.GetTagNamePart(" invalid:Tag"));

            exception.ShouldNotBeNull();
            exception.Message.ShouldBe("Tag Name contains invalid character “ ” at position 0.");

            _xPathValidator.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());
            _xPathValidator.Received(1).ValidateQName(" invalid:Tag", "Tag Name");
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateNCName(Arg.Any<string>(), Arg.Any<string>());

            _cssValidator.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());
        }

        [Fact]
        public void when_creating_xpath_local_tag_with_invalid_tag_name()
        {
            _xPathValidator.When(x => x.ValidateNCName(" invalidTag", "Tag Name"))
                .Do(x => throw new ArgumentException("Tag Name contains invalid character “ ” at position 0."));

            var exception = Should.Throw<ArgumentException>(() => _builder.GetTagNamePart(" invalidTag", isLocalName: true));

            exception.ShouldNotBeNull();
            exception.Message.ShouldBe("Tag Name contains invalid character “ ” at position 0.");

            _xPathValidator.DidNotReceiveWithAnyArgs().IsReservedFunctionName(Arg.Any<string>());
            _xPathValidator.DidNotReceiveWithAnyArgs().ValidateQName(Arg.Any<string>(), Arg.Any<string>());
            _xPathValidator.Received(1).ValidateNCName(" invalidTag", "Tag Name");

            _cssValidator.DidNotReceiveWithAnyArgs().ValidateClassName(Arg.Any<string>());
        }

        #endregion
    }
}