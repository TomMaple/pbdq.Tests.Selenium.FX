using System;
using pbdq.Tests.Selenium.FX.Helpers;
using Should;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.XPathBuilderTests
{
    public class XPathBuilder_GetTagNamePart_LocalName_Tests
    {
        [Fact]
        public void when_creating_xpath_tagName_from_null()
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart(null, true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentNullException>();
            exception.Message.ShouldEqual("Tag Name cannot be null.");
        }

        [Fact]
        public void when_creating_xpath_tagName_from_empty_text()
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart(string.Empty, true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Tag Name cannot be empty.");
        }

        [Fact]
        public void when_creating_xpath_tagName_containing_space()
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart("di v", true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Tag Name contains invalid character “ ”.");
        }

        [Fact]
        public void when_creating_xpath_tagName_starting_with_space()
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart(" div", true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Tag Name cannot start with “ ”.");
        }

        [Fact]
        public void when_creating_xpath_tagName_starting_with_colon()
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart(":div", true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Tag Name contains invalid character “:”.");
        }

        [Fact]
        public void when_creating_xpath_tagName_ending_with_colon()
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart("div:", true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Tag Name contains invalid character “:”.");
        }

        [Fact]
        public void when_creating_xpath_tagName_ending_with_space()
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart("div ", true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Tag Name contains invalid character “ ”.");
        }

        [Fact]
        public void when_creating_xpath_tagName_containing_tab()
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart("di\tv", true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Tag Name contains invalid character “\t”.");
        }

        [Fact]
        public void when_creating_xpath_tagName_containing_quotation_mark()
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart("div\"d\"", true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Tag Name contains invalid character “\"”.");
        }

        [Fact]
        public void when_creating_xpath_tagName_containing_apostrophe()
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart("john's", true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Tag Name contains invalid character “'”.");
        }

        [Fact]
        public void when_creating_xpath_tagName_from_single_letter()
        {
            var result = XPathBuilder.GetTagNamePart("a", true);
            result.ShouldEqual("*[local-name()='a']");
        }

        [Fact]
        public void when_creating_xpath_tagName_from_valid_tag_name()
        {
            var result = XPathBuilder.GetTagNamePart("div", true);
            result.ShouldEqual("*[local-name()='div']");
        }

        [Fact]
        public void when_creating_xpath_tagName_from_uppercase_valid_tag_name()
        {
            var result = XPathBuilder.GetTagNamePart("LI", true);
            result.ShouldEqual("*[local-name()='LI']");
        }

        [Fact]
        public void when_creating_xpath_tagName_from_prefixed_tag_name()
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart("abc:table", true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Tag Name contains invalid character “:”.");
        }

        [Fact]
        public void when_creating_xpath_tagName_starting_with_colons()
        {
            var exception = Record.Exception(() => XPathBuilder.GetTagNamePart("abc:def:div", true));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Tag Name contains invalid character “:”.");
        }

        [Fact]
        public void when_creating_xpath_tagName_with_dash()
        {
            var result = XPathBuilder.GetTagNamePart("book-title", true);
            result.ShouldEqual("*[local-name()='book-title']");
        }

        [Fact]
        public void when_creating_xpath_tagName_with_non_ASCII_characters()
        {
            var result = XPathBuilder.GetTagNamePart("书", true);
            result.ShouldEqual("*[local-name()='书']");
        }

        [Fact]
        public void when_creating_xpath_tagName_from_reserved_name()
        {
            var result = XPathBuilder.GetTagNamePart("text", true);
            result.ShouldEqual("*[local-name()='text']");
        }
    }
}