﻿using System;
using pbdq.Tests.Selenium.FX.Helpers;
using Shouldly;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.XPathBuilderTests
{
    public class XPathBuilder_GetTagNamePart_Tests
    {
        #region valid input data(

        [Theory]
        [InlineData(null, "*")]
        [InlineData("a", "a")]
        [InlineData("LI", "LI")]
        [InlineData("div", "div")]
        [InlineData("abc:table", "abc:table")]
        [InlineData("book-title", "book-title")]
        [InlineData("书", "书")]
        public void when_creating_xpath_tag_with_valid_values(string tagName, string expectedResult)
        {
            var result = XPathBuilder.GetTagNamePart(tagName);
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("a", "*[local-name()='a']")]
        [InlineData("LI", "*[local-name()='LI']")]
        [InlineData("div", "*[local-name()='div']")]
        [InlineData("book-title", "*[local-name()='book-title']")]
        [InlineData("书", "*[local-name()='书']")]
        public void when_creating_xpath_local_tag_with_valid_values(string tagName, string expectedResult)
        {
            var result = XPathBuilder.GetTagNamePart(tagName, isLocalName: true);
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("text", false, "*[name()='text']")]
        [InlineData("text", true, "*[local-name()='text']")]
        public void when_creating_xpath_tag_with_reserved_names(string tagName, bool isLocalName, string expectedResult)
        {
            var result = XPathBuilder.GetTagNamePart(tagName, isLocalName);
            result.ShouldBe(expectedResult);
        }

        #endregion

        #region invalid input data

        [Fact]
        public void when_creating_xpath_local_tag_with_null_value()
        {
            var exception = Should.Throw<ArgumentNullException>(() => XPathBuilder.GetTagNamePart(null, isLocalName: true));

            exception.ShouldNotBeNull();
            exception.Message.ShouldBe("Tag Name cannot be null.");
        }

        [Theory]
        [InlineData("", "Tag Name cannot be empty.")]
        [InlineData(" div", "Tag Name contains invalid character “ ” at position 0.")]
        [InlineData("di v", "Tag Name contains invalid character “ ” at position 2.")]
        [InlineData("div ", "Tag Name contains invalid character “ ” at position 3.")]
        [InlineData("di\tv", "Tag Name contains invalid character “\t” at position 2.")]
        [InlineData("di\"v", "Tag Name contains invalid character “\"” at position 2.")]
        [InlineData("di'v", "Tag Name contains invalid character “'” at position 2.")]
        [InlineData(":div", "Tag Name contains invalid character “:” at position 0.")]
        [InlineData("div:", "Tag Name contains invalid character “:” at position 3.")]
        [InlineData("abc::div", "Tag Name contains invalid character “:” at position 3.")]
        [InlineData("abc:div:", "Tag Name contains invalid character “:” at position 3.")]
        [InlineData("abc:def:div", "Tag Name contains invalid character “:” at position 3.")]
        public void when_creating_xpath_tag_with_invalid_tag_name(string tagName, string expectedErrorMessage)
        {
            var exception = Should.Throw<ArgumentException>(() => XPathBuilder.GetTagNamePart(tagName));

            exception.ShouldNotBeNull();
            exception.Message.ShouldBe(expectedErrorMessage);
        }

        [Theory]
        [InlineData("", "Tag Name cannot be empty.")]
        [InlineData(" div", "Tag Name contains invalid character “ ” at position 0.")]
        [InlineData("di v", "Tag Name contains invalid character “ ” at position 2.")]
        [InlineData("div ", "Tag Name contains invalid character “ ” at position 3.")]
        [InlineData("di\tv", "Tag Name contains invalid character “\t” at position 2.")]
        [InlineData("di\"v", "Tag Name contains invalid character “\"” at position 2.")]
        [InlineData("di'v", "Tag Name contains invalid character “'” at position 2.")]
        [InlineData(":div", "Tag Name contains invalid character “:” at position 0.")]
        [InlineData("di:v", "Tag Name contains invalid character “:” at position 2.")]
        [InlineData("div:", "Tag Name contains invalid character “:” at position 3.")]
        public void when_creating_xpath_local_tag_with_invalid_tag_name(string tagName, string expectedErrorMessage)
        {
            var exception = Should.Throw<ArgumentException>(() => XPathBuilder.GetTagNamePart(tagName, isLocalName: true));

            exception.ShouldNotBeNull();
            exception.Message.ShouldBe(expectedErrorMessage);
        }

        #endregion
    }
}