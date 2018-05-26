using System;
using pbdq.Tests.Selenium.FX.Helpers;
using Should;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.XPathBuilderTests
{
    public class XPathBuilder_GetAttributePart_Tests
    {
        [Fact]
        public void when_creating_xpath_attribute_with_nulls()
        {
            var exception = Record.Exception(() => XPathBuilder.GetAttributePart(null, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentNullException>();
            exception.Message.ShouldEqual("Attribute Name cannot be null.");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_empty_name()
        {
            var exception = Record.Exception(() => XPathBuilder.GetAttributePart(string.Empty, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Attribute Name cannot be empty.");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_name_containing_space()
        {
            var exception = Record.Exception(() => XPathBuilder.GetAttributePart("l ang", null));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Attribute Name contains invalid character “ ”.");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_name_starting_with_colon()
        {
            var exception = Record.Exception(() => XPathBuilder.GetAttributePart(":class", null));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Attribute Name prefix cannot be empty.");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_name_ending_with_colon()
        {
            var exception = Record.Exception(() => XPathBuilder.GetAttributePart("class:", null));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Attribute Name cannot be empty.");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_name_containing_tab()
        {
            var exception = Record.Exception(() => XPathBuilder.GetAttributePart("l\tang", null));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Attribute Name contains invalid character “\t”.");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_name_containing_quotation_mark()
        {
            var exception = Record.Exception(() => XPathBuilder.GetAttributePart("l\"ang", null));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Attribute Name contains invalid character “\"”.");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_name_containing_apostrophe()
        {
            var exception = Record.Exception(() => XPathBuilder.GetAttributePart("l'ang", null));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("Attribute Name contains invalid character “'”.");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_name_from_single_letter()
        {
            var result = XPathBuilder.GetAttributePart("a", null);
            result.ShouldEqual("[@a]");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_name_from_valid_attribute_name()
        {
            var result = XPathBuilder.GetAttributePart("class", null);
            result.ShouldEqual("[@class]");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_name_from_uppercase_valid_attribute_name()
        {
            var result = XPathBuilder.GetAttributePart("STYLE", null);
            result.ShouldEqual("[@STYLE]");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_name_from_prefixed_attribute_name()
        {
            var result = XPathBuilder.GetAttributePart("abc:class", null);
            result.ShouldEqual("[@abc:class]");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_colons()
        {
            var exception = Record.Exception(() => XPathBuilder.GetAttributePart("abc:def:div", null));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<FormatException>();
            exception.Message.ShouldEqual("Attribute Name cannot contain more than 1 colon.");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_dash()
        {
            var result = XPathBuilder.GetAttributePart("main-lang", null);
            result.ShouldEqual("[@main-lang]");
        }

        [Fact]
        public void when_creating_xpath_attribute_with_non_ASCII_characters()
        {
            var result = XPathBuilder.GetAttributePart("名字", null);
            result.ShouldEqual("[@名字]");
        }

        [Fact]
        public void when_creating_xpath_attribute_from_reserved_name()
        {
            var result = XPathBuilder.GetAttributePart("text", null);
            result.ShouldEqual("[@text]");
        }
    }
}