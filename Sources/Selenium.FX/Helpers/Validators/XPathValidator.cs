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
        //private static readonly string NameStartChar = "[:A-Z_a-z\xC0-\xD6\xD8-\xF6\xF8-\x2FF\x370-\x37D\x37F-\x1FFF\x200C-\x200D\x2070-\x218F\x2C00-\x2FEF\x3001-\xD7FF\xF900-\xFDCF\xFDF0-\xFFFD\x10000-\xEFFFF]";
        private static readonly string NameStartCharRegex = ":A-Z_a-z\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02FF\u0370-\u037D\u037F-\u1FFF\u200C-\u200D\u2070-\u218F\u2C00-\u2FEF\u3001-\uD7FF\uF900-\uFDCF\uFDF0-\uFFFD";
        private static readonly string NameCharRegex = $"-.0-9{NameStartCharRegex}\u00B7\u0300-\u036F\u203F-\u2040";
        //private static readonly string NameRegex = $"{NameStartCharRegex}({NameCharRegex})*";
        //private static readonly string CharRegex = "\x9\xA\xD\x20-\xD7FF\xE000-\xFFFD";
        //private static readonly string NcNameRegex = $"({NameRegex}-[{CharRegex}|:])"; // # TODO: update
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

                throw new NotImplementedException($"{elementTypeName} is invalid.");
            }

            //var parts = qName.Split(':');
            //if (parts.Length > 2)
            //    throw new FormatException($"{elementTypeName} cannot contain more than 1 colon.");
            //if (parts[0] == string.Empty)
            //    throw new FormatException($"{elementTypeName} prefix cannot be empty.", null);
            //if (parts.Length > 1 && parts[1] == string.Empty)
            //    throw new FormatException($"{elementTypeName} cannot be empty.", null);

            //foreach (var part in parts)
            //    ValidateNCName(part, elementTypeName);
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

                throw new NotImplementedException($"{elementTypeName} is invalid.");
            }

            //if (ncName.IndexOf(':') != -1)
            //    throw new ArgumentException($"{elementTypeName} contains invalid character “:”.", NullException);

            //for (var i = 0; i < ncName.Length; i++)
            //{
            //    if (IsNameChar(ncName[i]) == false)
            //        throw new ArgumentException($"{elementTypeName} contains invalid character “{ncName[i]}”.",
            //            NullException);

            //    if (i == 0 && IsNameStartChar(ncName[i]) == false)
            //        throw new ArgumentException($"{elementTypeName} cannot start with “{ncName[0]}”.", NullException);
            //}
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
            //var nameCharRegex = new Regex($"[{NameCharRegex}]");
            var nameCharRegex = new Regex(NcNameCharRegex);
            if (nameCharRegex.IsMatch(character.ToString()) == false)
                throw new ArgumentException($"{elementTypeName} contains invalid character “{character}” at position 0.");

            //var nameStartCharRegex = new Regex($"[{NameStartCharRegex}]");
            var nameStartCharRegex = new Regex(NcNameStartCharRegex);
            if (nameStartCharRegex.IsMatch(character.ToString()) == false)
                throw new ArgumentException($"{elementTypeName} cannot start with “{character}”.");
        }

        private static void ValidateQCharacters(string name, string elementTypeName)
        {
            var regex = new Regex(QNameRegex);

            for (var idx = 1; idx < name.Length; idx++)
            {
                if(regex.IsMatch(name.Substring(0, idx+1))==false)
                    throw new ArgumentException($"{elementTypeName} contains invalid character “{name[idx]}” at position {idx}.");
            }
        }

        private static void ValidateNcCharacters(string name, string elementTypeName)
        {
            var regex = new Regex($"^{NcNameRegex}$");

            for (var idx = 1; idx < name.Length; idx++)
            {
                if(regex.IsMatch(name.Substring(0, idx+1))==false)
                    throw new ArgumentException($"{elementTypeName} contains invalid character “{name[idx]}” at position {idx}.");
            }
        }

        /// <remarks>According to https://www.w3.org/TR/REC-xml/#NT-NameStartChar</remarks>
        private static bool IsNameStartChar(char character)
        {
            if (character == ':')
                return true;

            if (character >= 'A' && character <= 'Z')
                return true;

            if (character == '_')
                return true;

            if (character >= 'a' && character <= 'z')
                return true;

            if (character >= 0xC0 && character <= 0xD6)
                return true;

            if (character >= 0xD8 && character <= 0xF6)
                return true;

            if (character >= 0xF8 && character <= 0x2FF)
                return true;

            if (character >= 0x370 && character <= 0x37D)
                return true;

            if (character >= 0x37F && character <= 0x1FFF)
                return true;

            if (character >= 0x200C && character <= 0x200D)
                return true;

            if (character >= 0x2070 && character <= 0x218F)
                return true;

            if (character >= 0x2C00 && character <= 0x2FEF)
                return true;

            if (character >= 0x3001 && character <= 0xD7FF)
                return true;

            if (character >= 0xF900 && character <= 0xFDCF)
                return true;

            if (character >= 0xFDF0 && character <= 0xFFFD)
                return true;

            if (character >= 0x10000 && character <= 0xEFFFF)
                return true;

            return false;
        }

        /// <remarks>According to https://www.w3.org/TR/REC-xml/#NT-NameChar</remarks>
        private static bool IsNameChar(char character)
        {
            if (IsNameStartChar(character))
                return true;

            if (character == '-' || character == '.')
                return true;

            if (character >= '0' && character <= '9')
                return true;

            if (character == 0xB7)
                return true;

            if (character >= 0x0300 && character <= 0x036F)
                return true;

            if (character >= 0x203F && character <= 0x2040)
                return true;

            return false;
        }
    }
}