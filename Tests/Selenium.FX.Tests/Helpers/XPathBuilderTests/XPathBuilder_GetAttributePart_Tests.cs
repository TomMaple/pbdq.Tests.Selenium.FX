using System;
using pbdq.Tests.Selenium.FX.Helpers;
using Should;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.XPathBuilderTests
{
    public class XPathBuilder_GetAttributePart_Tests
    {
        #region valid input data

        [Theory]
        [InlineData(null, null, "[@*]")]
        [InlineData(null, "", "[@*='']")]
        [InlineData(null, "en", "[@*='en']")]
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
            var result = XPathBuilder.GetAttributePart(attributeName, attributeValue);
            result.ShouldEqual(expectedResult);
        }

        [Theory]
        [InlineData("text", "text", "[@text='text']")]
        public void when_creating_xpath_attribute_with_reserved_names(string attributeName, string attributeValue, string expectedResult)
        {
            var result = XPathBuilder.GetAttributePart(attributeName, attributeValue);
            result.ShouldEqual(expectedResult);
        }

        #endregion

        #region invalid input data

        [Theory]
        [InlineData("", "Attribute Name cannot be empty.")]
        [InlineData(" lang", "Attribute Name contains invalid character “ ” at position 0.")]
        [InlineData("l ang", "Attribute Name contains invalid character “ ” at position 1.")]
        [InlineData("l\tang", "Attribute Name contains invalid character “\t” at position 1.")]
        [InlineData("lang ", "Attribute Name contains invalid character “ ” at position 4.")]
        [InlineData("l'ang", "Attribute Name contains invalid character “'” at position 1.")]
        [InlineData("l\"ang", "Attribute Name contains invalid character “\"” at position 1.")]
        [InlineData("*", "Attribute Name contains invalid character “*” at position 0.")]
        [InlineData("1lang", "Attribute Name cannot start with “1”.")]
        [InlineData("-lang", "Attribute Name cannot start with “-”.")]
        public void when_creating_xpath_attribute_with_invalid_attibute_name(string name, string expectedErrorMessage)
        {
            var exception = Record.Exception(() => XPathBuilder.GetAttributePart(name, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual(expectedErrorMessage);
        }

        [Theory]
        [InlineData(":abc", "Attribute Name contains invalid character “:” at position 0.")]
        [InlineData("abc:", "Attribute Name contains invalid character “:” at position 3.")]
        [InlineData("abc:def:", "Attribute Name contains invalid character “:” at position 3.")]
        [InlineData("abc::def", "Attribute Name contains invalid character “:” at position 3.")]
        [InlineData("abc:def:div", "Attribute Name contains invalid character “:” at position 3.")]
        public void when_creating_xpath_attribute_with_invalid_format(string name, string expectedErrorMessage)
        {
            var exception = Record.Exception(() => XPathBuilder.GetAttributePart(name, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<FormatException>();
            exception.Message.ShouldEqual(expectedErrorMessage);
        }
        
        #endregion
    }
}