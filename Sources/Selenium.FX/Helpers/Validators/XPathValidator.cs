using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace pbdq.Tests.Selenium.FX.Helpers.Validators
{
    internal static class XPathValidator
    {
        private static readonly Exception NullException = null;

        //// https://www.regular-expressions.info/shorthand.html

        // TODO: update to add UTF-32 characters
        //private static readonly string NameStartCharRegex = "[:A-Z_a-z\xC0-\xD6\xD8-\xF6\xF8-\x2FF\x370-\x37D\x37F-\x1FFF\x200C-\x200D\x2070-\x218F\x2C00-\x2FEF\x3001-\xD7FF\xF900-\xFDCF\xFDF0-\xFFFD\x10000-\xEFFFF]";
        private static readonly string NameStartCharRegex = ":A-Z_a-z\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02FF\u0370-\u037D\u037F-\u1FFF\u200C-\u200D\u2070-\u218F\u2C00-\u2FEF\u3001-\uD7FF\uF900-\uFDCF\uFDF0-\uFFFD";
        private static readonly string NameCharRegex = $"-.0-9{NameStartCharRegex}\u00B7\u0300-\u036F\u203F-\u2040";
        private static readonly string NcNameStartCharRegex = $"[{NameStartCharRegex}-[:]]";
        private static readonly string NcNameCharRegex = $"[{NameCharRegex}-[:]]";
        private static readonly string NcNameRegex = $"({NcNameStartCharRegex}({NcNameCharRegex})*)";
        private static readonly string PrefixedNameRegex = $"({NcNameRegex}:{NcNameRegex})";
        private static readonly string QNameRegex = $"^({PrefixedNameRegex}|{NcNameRegex})$";

        private static readonly string[] ReservedFunctionNames =
        {
            "attribute", "comment", "document-node", "element", "empty-sequence", "if", "item", "node",
            "processing-instruction", "schema-attribute", "text", "typeswitch"
        };

        /// <remarks>Based on:
        /// https://www.w3.org/TR/REC-xml-names/#NT-QName
        /// </remarks>
        internal static void ValidateQName(string qName, string elementTypeName)
        {
            if (qName == null)
                throw new ArgumentNullException($"{elementTypeName} cannot be null.", NullException);

            if (qName == string.Empty)
                throw new ArgumentException($"{elementTypeName} cannot be empty.", NullException);

            var regex = new Regex(QNameRegex);
            if (regex.IsMatch(qName) == false)
            {
                ValidateFirstCharacter(qName[0], elementTypeName);
                ValidateQCharacters(qName, elementTypeName);

                throw new ArgumentException($"{elementTypeName} contains invalid character “{qName[1]}” at position 1.");
            }
        }

        internal static void ValidateNCName(string ncName, string elementTypeName)
        {
            if (ncName == null)
                throw new ArgumentNullException($"{elementTypeName} cannot be null.", NullException);

            if (ncName == string.Empty)
                throw new ArgumentException($"{elementTypeName} cannot be empty.", NullException);

            var regex = new Regex($"^{NcNameRegex}$");
            if (regex.IsMatch(ncName) == false)
            {
                ValidateFirstCharacter(ncName[0], elementTypeName);
                ValidateNcCharacters(ncName, elementTypeName);

                throw new ArgumentException($"{elementTypeName} contains invalid character “{ncName[1]}” at position 1.");
            }
        }

        internal static bool IsReservedFunctionName(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), "Name cannot be null!");

            var parts = name.Split(':');
            return parts.Any(part => ReservedFunctionNames.Contains(part.ToLower()));
        }

        private static void ValidateFirstCharacter(char character, string elementTypeName)
        {
            var nameCharRegex = new Regex(NcNameCharRegex);
            if (nameCharRegex.IsMatch(character.ToString()) == false)
                throw new ArgumentException($"{elementTypeName} contains invalid character “{character}” at position 0.");

            var nameStartCharRegex = new Regex(NcNameStartCharRegex);
            if (nameStartCharRegex.IsMatch(character.ToString()) == false)
                throw new ArgumentException($"{elementTypeName} cannot start with “{character}”.");
        }

        private static void ValidateQCharacters(string name, string elementTypeName)
        {
            var regex = new Regex(QNameRegex);
            var maxLength = name.Length;

            for (var length = maxLength; length > 0; length--)
            {
                if(regex.IsMatch(name.Substring(0, length)) && length < maxLength)
                    throw new ArgumentException($"{elementTypeName} contains invalid character “{name[length]}” at position {length}.");
            }
        }

        private static void ValidateNcCharacters(string name, string elementTypeName)
        {
            var regex = new Regex($"^{NcNameRegex}$");
            var maxLength = name.Length;

            for (var length = maxLength; length > 0; length--)
            {
                if (regex.IsMatch(name.Substring(0, length)) && length < maxLength)
                    throw new ArgumentException($"{elementTypeName} contains invalid character “{name[length]}” at position {length}.");
            }
        }
    }
}