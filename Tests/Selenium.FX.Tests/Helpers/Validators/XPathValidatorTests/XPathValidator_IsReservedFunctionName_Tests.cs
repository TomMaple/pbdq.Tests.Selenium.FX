using System;
using pbdq.Tests.Selenium.FX.Helpers.Validators;
using Shouldly;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.Validators.XPathValidatorTests
{
    public class XPathValidator_IsReservedFunctionName_Tests
    {
        private readonly XPathValidator _validator;

        public XPathValidator_IsReservedFunctionName_Tests()
        {
            _validator = new XPathValidator();
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("a4")]
        [InlineData("")]
        [InlineData("AaA")]
        public void when_validating_if_is_reserved_function_name_with_valid_values(string name)
        {
            var result = _validator.IsReservedFunctionName(name);
            result.ShouldBeFalse();
        }

        [Fact]
        public void when_validating_null_if_is_reserved_function_name()
        {
            var exception = Should.Throw<ArgumentNullException>(() => _validator.IsReservedFunctionName(null));

            exception.ShouldNotBeNull();
            exception.Message.ShouldStartWith("Name cannot be null!");
        }

        [Theory]
        [InlineData("attribute")]
        [InlineData("comment")]
        [InlineData("document-node")]
        [InlineData("element")]
        [InlineData("empty-sequence")]
        [InlineData("if")]
        [InlineData("item")]
        [InlineData("node")]
        [InlineData("processing-instruction")]
        [InlineData("schema-attribute")]
        [InlineData("text")]
        [InlineData("typeswitch")]
        [InlineData("Attribute")]
        [InlineData("ATTRIBUTE")]
        public void when_validating_if_is_reserved_function_name_with_invalid_values(string name)
        {
            var result = _validator.IsReservedFunctionName(name);
            result.ShouldBeTrue();
        }
    }
}