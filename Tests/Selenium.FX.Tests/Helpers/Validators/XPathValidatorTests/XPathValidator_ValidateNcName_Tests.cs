using System;
using pbdq.Tests.Selenium.FX.Helpers.Validators;
using Shouldly;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.Validators.XPathValidatorTests
{
    public class XPathValidator_ValidateNcName_Tests
    {
        private readonly XPathValidator _validator;

        public XPathValidator_ValidateNcName_Tests()
        {
            _validator = new XPathValidator();
        }

        [Theory]
        [InlineData("a")]
        [InlineData("LI")]
        [InlineData("div")]
        [InlineData("text")]
        [InlineData("book-title")]
        [InlineData("book_title")]
        [InlineData("_test")]
        [InlineData("book.title")]
        [InlineData("a9")]
        [InlineData("á")]
        [InlineData("书")]
        public void when_validating_ncName_with_valid_values(string qName)
        {
            Should.NotThrow(() => _validator.ValidateNCName(qName, "Tag Name"));
        }

        [Fact]
        public void when_validating_ncName_with_null_value()
        {
            var exception = Should.Throw<ArgumentNullException>(() => _validator.ValidateNCName(null, "Tag Name"));
            exception.ShouldNotBeNull();
            exception.Message.ShouldBe("Tag Name cannot be null.");
        }

        [Theory]
        [InlineData("", "Tag Name cannot be empty.")]
        [InlineData(" ", "Tag Name contains invalid character “ ” at position 0.")]
        [InlineData(" div", "Tag Name contains invalid character “ ” at position 0.")]
        [InlineData("di v", "Tag Name contains invalid character “ ” at position 2.")]
        [InlineData("div ", "Tag Name contains invalid character “ ” at position 3.")]
        [InlineData("9", "Tag Name cannot start with “9”.")]
        [InlineData("9div", "Tag Name cannot start with “9”.")]
        [InlineData(".div", "Tag Name cannot start with “.”.")]
        [InlineData("-div", "Tag Name cannot start with “-”.")]
        [InlineData("di\tv", "Tag Name contains invalid character “\t” at position 2.")]
        [InlineData("di\"v", "Tag Name contains invalid character “\"” at position 2.")]
        [InlineData("di'v", "Tag Name contains invalid character “'” at position 2.")]
        [InlineData(":div", "Tag Name contains invalid character “:” at position 0.")]
        [InlineData("div:", "Tag Name contains invalid character “:” at position 3.")]
        [InlineData("abc::div", "Tag Name contains invalid character “:” at position 3.")]
        [InlineData("abc:div", "Tag Name contains invalid character “:” at position 3.")]
        [InlineData("abc:div:", "Tag Name contains invalid character “:” at position 3.")]
        [InlineData("abc:def:div", "Tag Name contains invalid character “:” at position 3.")]
        public void when_validating_ncName_with_invalid_values(string qName, string expectedErrorMessage)
        {
            var exception = Should.Throw<ArgumentException>(() => _validator.ValidateNCName(qName, "Tag Name"));
            exception.ShouldNotBeNull();
            exception.Message.ShouldBe(expectedErrorMessage);
        }
    }
}