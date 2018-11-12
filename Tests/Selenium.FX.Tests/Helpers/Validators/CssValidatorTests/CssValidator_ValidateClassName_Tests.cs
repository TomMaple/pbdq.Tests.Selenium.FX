using System;
using pbdq.Tests.Selenium.FX.Helpers.Validators;
using Shouldly;
using Xunit;

namespace pbdq.Tests.Selenium.FX.Tests.Helpers.Validators.CssValidatorTests
{
    public class CssValidator_ValidateClassName_Tests
    {
        private readonly CssValidator _validator;

        public CssValidator_ValidateClassName_Tests()
        {
            _validator = new CssValidator();
        }

        [Theory]
        [InlineData("a")]
        [InlineData("LI")]
        [InlineData("button")]
        [InlineData("primary-button")]
        [InlineData("primary_button")]
        [InlineData("-a")]
        [InlineData("-a-")]
        [InlineData("_a")]
        [InlineData("a9")]
        [InlineData("Á")]
        [InlineData("书")]
        public void when_creating_xpath_css_class_with_valid_values(string cssClass)
        {
            Should.NotThrow(() => _validator.ValidateClassName(cssClass));
        }

        [Fact]
        public void when_validating_class_name_with_null_value()
        {
            var exception = Should.Throw<ArgumentNullException>(() => _validator.ValidateClassName(null));

            exception.ShouldNotBeNull();
            exception.Message.ShouldStartWith("CSS Class Name cannot be null.");
        }

        [Theory]
        [InlineData("", "CSS Class Name cannot be empty.")]
        [InlineData(" panel", "CSS Class Name contains invalid character “ ” at position 0.")]
        [InlineData("panel1 panel2", "CSS Class Name contains invalid character “ ” at position 6.")]
        [InlineData("panel ", "CSS Class Name contains invalid character “ ” at position 5.")]
        [InlineData("panel\t", "CSS Class Name contains invalid character “\t” at position 5.")]
        [InlineData("panel\"", "CSS Class Name contains invalid character “\"” at position 5.")]
        [InlineData("panel\'", "CSS Class Name contains invalid character “\'” at position 5.")]
        [InlineData("9panel", "CSS Class Name cannot start with “9”.")]
        [InlineData("--", "CSS Class Name contains invalid character “-” at position 1.")]
        [InlineData("-9", "CSS Class Name contains invalid character “9” at position 1.")]
        public void when_validating_class_name_with_invalid_value(string className, string expectedErrorMessage)
        {
            var exception = Should.Throw<ArgumentException>(() => _validator.ValidateClassName(className));

            exception.ShouldNotBeNull();
            exception.Message.ShouldBe(expectedErrorMessage);
        }
    }
}