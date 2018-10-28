using System;
using System.Text.RegularExpressions;

namespace pbdq.Tests.Selenium.FX.Helpers.Validators
{
    internal static class CssValidator
    {
        // TODO: update to add UTF-32 characters
        // private static readonly string NonAsciiRegex = "[\u0080-\uD7FF\uE000-\uFFFD\U00010000-\U0010FFFF]";
        // private static readonly string EscapeRegex = $"{unicodeRegex}|\\\\[\u0020-\u007E\u0080-\uD7FF\uE000-\uFFFD\U00010000-\U0010FFFF]";

        private static readonly string NonAsciiRegex = "[\u0080-\uD7FF\uE000-\uFFFD]";
        private static readonly string UnicodeRegex = "\\[0-9a-fA-F]{1,6}[\t\n\f\r ]?";
        private static readonly string EscapeRegex = $"{UnicodeRegex}|\\\\[\u0020-\u007E\u0080-\uD7FF\uE000-\uFFFD]";
        private static readonly string NmStartRegex = $"([_a-zA-Z]|{NonAsciiRegex}|{EscapeRegex})";
        private static readonly string NmCharRegex = $"([_a-zA-Z0-9-]|{NonAsciiRegex}|{EscapeRegex})";
        private static readonly string IdentRegex = $"^-?{NmStartRegex}{NmCharRegex}*$";

        /// <remarks>
        ///     Based on:
        ///     https://www.w3.org/TR/CSS2/grammar.html
        ///     https://www.w3.org/TR/css-syntax-3/#typedef-ident-token
        ///     https://github.com/gorilla/css/blob/master/scanner/scanner.go
        /// </remarks>
        internal static void ValidateClassName(string className)
        {
            if (className == null)
                throw new ArgumentNullException("CSS Class Name cannot be null.", (Exception) null);

            if (className == string.Empty)
                throw new ArgumentException("CSS Class Name cannot be empty.", (Exception) null);

            var regex = new Regex(IdentRegex);
            if (regex.IsMatch(className) == false)
            {
                ValidateFirstCharacter(className[0]);
                ValidateCharacters(className);

                throw new ArgumentException($"CSS Class Name contains invalid character “{className[1]}” at position 1.");
            }
        }

        private static void ValidateFirstCharacter(char character)
        {
            if (character == '-')
                return;

            var nmCharRegex = new Regex(NmCharRegex);
            if (nmCharRegex.IsMatch(character.ToString()) == false)
                throw new ArgumentException($"CSS Class Name contains invalid character “{character}” at position 0.");

            var nmStartRegex = new Regex(NmStartRegex);
            if (nmStartRegex.IsMatch(character.ToString()) == false)
                throw new ArgumentException($"CSS Class Name cannot start with “{character}”.");
        }

        private static void ValidateCharacters(string className)
        {
            var regex = new Regex(IdentRegex);
            var maxLength = className.Length;

            for (var length = maxLength; length > 0; length--)
                if (regex.IsMatch(className.Substring(0, length)) && length < maxLength)
                    throw new ArgumentException($"CSS Class Name contains invalid character “{className[length]}” at position {length}.");
        }
    }
}