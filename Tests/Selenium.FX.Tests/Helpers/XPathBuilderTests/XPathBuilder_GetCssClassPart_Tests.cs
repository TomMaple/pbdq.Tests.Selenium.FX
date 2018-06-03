using System;
using pbdq.Tests.Selenium.FX.Helpers;
using Should;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.XPathBuilderTests
{
    public class XPathBuilder_GetCssClassPart_Tests
    {
        [Theory]
        [InlineData("a", "[contains(concat(' ',normalize-space(@class),' '),' a ')]")]
        [InlineData("LI", "[contains(concat(' ',normalize-space(@class),' '),' LI ')]")]
        [InlineData("button", "[contains(concat(' ',normalize-space(@class),' '),' button ')]")]
        [InlineData("primary-button", "[contains(concat(' ',normalize-space(@class),' '),' primary-button ')]")]
        [InlineData("primary_button", "[contains(concat(' ',normalize-space(@class),' '),' primary_button ')]")]
        [InlineData("书", "[contains(concat(' ',normalize-space(@class),' '),' 书 ')]")]
        public void when_creating_xpath_css_class_with_valid_values(string cssClass, string expectedResult)
        {
            var result = XPathBuilder.GetCssClassPart(cssClass);
            result.ShouldEqual(expectedResult);
        }

        [Fact]
        public void when_creating_xpath_css_class_with_null_value()
        {
            var exception = Record.Exception(() => XPathBuilder.GetCssClassPart(null));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentNullException>();
            exception.Message.ShouldEqual("CSS Class Name cannot be null.");
        }

        [Fact]
        public void when_creating_xpath_css_class_with_invalid_value()
        {
            var exception = Record.Exception(() => XPathBuilder.GetCssClassPart(" invalid value "));

            exception.ShouldNotBeNull();
            exception.ShouldBeType<ArgumentException>();
            exception.Message.ShouldEqual("CSS Class Name contains invalid character “ ” at position 0.");
        }
    }
}