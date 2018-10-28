using System;
using pbdq.Tests.Selenium.FX.Helpers.Validators;
using Should;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.Validators.XPathValidatorTests
{
    public class XPathValidator_IsReservedFunctionName_Tests
    {
        [Theory]
        [InlineData("abc")]
        [InlineData("a4")]
        [InlineData("")]
        [InlineData("AaA")]
        public void when_validating_if_is_reserved_function_name_with_valid_values(string name)
        {
            var result = XPathValidator.IsReservedFunctionName(name);
            result.ShouldBeFalse();
        }

        [Fact]
        public void when_validating_null_if_is_reserved_function_name()
        {
            var exception = Record.Exception(() => XPathValidator.IsReservedFunctionName(null));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentNullException>();
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
            var result = XPathValidator.IsReservedFunctionName(name);
            result.ShouldBeTrue();
        }
    }
}