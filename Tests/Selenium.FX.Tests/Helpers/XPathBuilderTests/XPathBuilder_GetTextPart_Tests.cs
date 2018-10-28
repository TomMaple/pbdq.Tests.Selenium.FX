using pbdq.Tests.Selenium.FX.Helpers;
using Shouldly;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.XPathBuilderTests
{
    public class XPathBuilder_GetTextPart_Tests
    {
        [Theory]
        [InlineData(null, "[text()]")]
        [InlineData("a", "[text()='a']")]
        [InlineData("6", "[text()='6']")]
        [InlineData("abc", "[text()='abc']")]
        [InlineData("a c", "[text()='a c']")]
        [InlineData("ABC", "[text()='ABC']")]
        [InlineData("abc:def", "[text()='abc:def']")]
        [InlineData("名字", "[text()='名字']")]
        [InlineData("abc \"def\"", "[text()='abc \"def\"']")]
        [InlineData("abc 'def'", "[text()='abc &apos;def&apos;']")]
        [InlineData("x > 9", "[text()='x &gt; 9']")]
        public void when_creating_text_with_valid_values(string text, string expectedResult)
        {
            var result = XPathBuilder.GetTextPart(text);
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData(null, "[text()]")]
        [InlineData("a", "[contains(text(),'a')]")]
        [InlineData("6", "[contains(text(),'6')]")]
        [InlineData("abc", "[contains(text(),'abc')]")]
        [InlineData("a c", "[contains(text(),'a c')]")]
        [InlineData("ABC", "[contains(text(),'ABC')]")]
        [InlineData("abc:def", "[contains(text(),'abc:def')]")]
        [InlineData("名字", "[contains(text(),'名字')]")]
        [InlineData("abc \"def\"", "[contains(text(),'abc \"def\"')]")]
        [InlineData("abc 'def'", "[contains(text(),'abc &apos;def&apos;')]")]
        [InlineData("x > 9", "[contains(text(),'x &gt; 9')]")]
        public void when_creating_partial_text_with_valid_values(string text, string expectedResult)
        {
            var result = XPathBuilder.GetTextPart(text, true);
            result.ShouldBe(expectedResult);
        }
    }
}