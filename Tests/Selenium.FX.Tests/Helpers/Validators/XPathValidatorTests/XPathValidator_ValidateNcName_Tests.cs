﻿using System;
using pbdq.Tests.Selenium.FX.Helpers.Validators;
using Should;
using Xunit;
using Record = Should.Core.Assertions.Record;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.Validators.XPathValidatorTests
{
    public class XPathValidator_ValidateNcName_Tests
    {
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
        public void when_validating_qName_with_valid_values(string qName)
        {
            var exception = Record.Exception(() => XPathValidator.ValidateNCName(qName, "Tag Name"));
            exception.ShouldBeNull();
        }

        [Fact]
        public void when_validating_qName_with_null_value()
        {
            var exception = Record.Exception(() => XPathValidator.ValidateNCName(null, "Tag Name"));
            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentNullException>();
            exception.Message.ShouldEqual("Tag Name cannot be null.");
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
        public void when_validating_qName_with_invalid_values(string qName, string expectedErrorMessage)
        {
            var exception = Record.Exception(() => XPathValidator.ValidateNCName(qName, "Tag Name"));
            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual(expectedErrorMessage);
        }
    }
}