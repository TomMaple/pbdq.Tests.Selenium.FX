using System;
using pbdq.Tests.Selenium.FX.Helpers;
using Should;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.XPathBuilderTests
{
    public class XPathBuilder_GetTagNamePart_Tests
    {
        #region valid input data

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
            result.ShouldEqual(expectedResult);
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
            result.ShouldEqual(expectedResult);
        }

        [Theory]
        [InlineData("text", false, "*[name()='text']")]
        [InlineData("text", true, "*[local-name()='text']")]
        public void when_creating_xpath_tag_with_reserved_names(string tagName, bool isLocalName, string expectedResult)
        {
            var result = XPathBuilder.GetTagNamePart(tagName, isLocalName);
            result.ShouldEqual(expectedResult);
        }

        #endregion

        #region invalid input data

        [Fact]
        public void when_creating_xpath_local_tag_with_null_value()
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart(null, isLocalName: true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentNullException>();
            exception.Message.ShouldEqual("Tag Name cannot be null.");
        }

        [Theory]
        [InlineData("", "Tag Name cannot be empty.")]
        [InlineData(" div", "Tag Name contains invalid character “ ”.")]
        [InlineData("di v", "Tag Name contains invalid character “ ”.")]
        [InlineData("div ", "Tag Name contains invalid character “ ”.")]
        [InlineData("di\tv", "Tag Name contains invalid character “\t”.")]
        [InlineData("di\"v", "Tag Name contains invalid character “\"”.")]
        [InlineData("di'v", "Tag Name contains invalid character “'”.")]
        public void when_creating_xpath_tag_with_invalid_tag_name(string tagName, string expectedErrorMessage)
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart(tagName));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual(expectedErrorMessage);
        }

        [Theory]
        [InlineData("", "Tag Name cannot be empty.")]
        [InlineData(" div", "Tag Name contains invalid character “ ”.")]
        [InlineData("di v", "Tag Name contains invalid character “ ”.")]
        [InlineData("div ", "Tag Name contains invalid character “ ”.")]
        [InlineData("di\tv", "Tag Name contains invalid character “\t”.")]
        [InlineData("di\"v", "Tag Name contains invalid character “\"”.")]
        [InlineData("di'v", "Tag Name contains invalid character “'”.")]
        [InlineData(":div", "Tag Name contains invalid character “:”.")]
        [InlineData("di:v", "Tag Name contains invalid character “:”.")]
        [InlineData("div:", "Tag Name contains invalid character “:”.")]
        public void when_creating_xpath_local_tag_with_invalid_tag_name(string tagName, string expectedErrorMessage)
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart(tagName, isLocalName: true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual(expectedErrorMessage);
        }

        [Theory]
        [InlineData(":div", "Tag Name prefix cannot be empty.")]
        [InlineData("div:", "Tag Name cannot be empty.")]
        [InlineData("abc::div", "Tag Name cannot contain more than 1 colon.")]
        [InlineData("abc:div:", "Tag Name cannot contain more than 1 colon.")]
        [InlineData("abc:def:div", "Tag Name cannot contain more than 1 colon.")]
        public void when_creating_xpath_tag_with_invalid_format(string tagName, string expectedErrorMessage)
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart(tagName));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<FormatException>();
            exception.Message.ShouldEqual(expectedErrorMessage);
        }

        #endregion
    }
}